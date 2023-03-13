using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewFile : MonoBehaviour
{
    public static string filePath;
    
    public static string mp3Location;
    public static string midiLocation;

    [SerializeField] private TMP_Text fileName;
    [SerializeField] private TMP_InputField song;
    [SerializeField] private TMP_InputField artist;
    [SerializeField] private Button button;
    
    // Start is called before the first frame update
    void Start()
    {
        button.gameObject.SetActive(false);
        //check if name is unique
        fileName.text = Path.GetFileName(filePath);
        
        Debug.Log("path " + filePath);
        ConvertMp3ToMidi(filePath, button);
    }
    
    public void ConvertMp3ToMidi(string Mp3FilePath, Button playButton)
    {
        StartCoroutine(UploadMp3AndDownloadMidi(Mp3FilePath, playButton));
    }

    private IEnumerator UploadMp3AndDownloadMidi(string Mp3FilePath, Button playButton)
    {
        var form = new WWWForm();
        var mp3Bytes = File.ReadAllBytes(Mp3FilePath);

        string mp3FileName = Path.GetFileName(Mp3FilePath);
        form.AddBinaryData("file", mp3Bytes, mp3FileName, "audio/mpeg");
        
        string mp3Path = Application.persistentDataPath + "/mp3/" + mp3FileName;
        System.IO.File.WriteAllBytes(mp3Path, mp3Bytes);

        NewFile.mp3Location = mp3Path;
        
        using (var request = UnityWebRequest.Post("https://dannysantanasf.pythonanywhere.com/upload", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string MidiFilePath = Application.persistentDataPath + "/midi/" + Path.GetFileNameWithoutExtension(Mp3FilePath) + ".mid";
                var midiBytes = request.downloadHandler.data;
                File.WriteAllBytes(MidiFilePath, midiBytes);
                NewFile.midiLocation = MidiFilePath;
                playButton.gameObject.SetActive(true);
            }
        }
    }


    public const string seperator = "$!&(^$)%!";
    public const string entry_seperator = ")%J)H$%\n";
    
    public void playPressed()
    {
        if (!string.IsNullOrEmpty(song.text) && !string.IsNullOrEmpty(artist.text))
        {
            string combinedTitle = song.text + " - " + artist.text;
            PlayerPrefs.SetString(combinedTitle, mp3Location + midiLocation);
            
            string fileName = "songs.txt"; 
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            
            string textToWrite = entry_seperator + combinedTitle + seperator + mp3Location + seperator + midiLocation + seperator;
            
            StreamWriter writer = new StreamWriter(filePath, true);
            
            writer.WriteLine(textToWrite);
            writer.Close();
            
            StartCoroutine(GameManager.GetSongAudioClipThenStartGame(mp3Location, midiLocation));
        }
    }
}
