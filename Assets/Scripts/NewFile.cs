using System;
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
    
    private string mp3Location;
    private string midiLocation;

    [SerializeField] private TMP_Text fileName;
    [SerializeField] private TMP_InputField song;
    [SerializeField] private TMP_InputField artist;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text pitchShiftText;

    [SerializeField] private GameObject loadingAnimation;
    
    private int key = 0;
    private int desiredKey = 0;
    private int pitchShift = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        button.gameObject.SetActive(false);
        if (filePath != null)
        {
            //check if name is unique
            fileName.text = Path.GetFileName(filePath);
        
            Debug.Log("path " + filePath);
            ConvertMp3ToMidi(filePath, button);
        }
    }
    
    public void ConvertMp3ToMidi(string Mp3FilePath, Button playButton)
    {
        StartCoroutine(UploadMp3AndDownloadMidi(Mp3FilePath, playButton));
    }

    public string GenerateUniqueFilename(string originalFilename)
    {
        string uniqueFilename = Path.GetFileNameWithoutExtension(originalFilename) + "_" + DateTime.UtcNow.ToString("yyyyMMddTHHmmss") + Path.GetExtension(originalFilename);
        return uniqueFilename;
    }

    private IEnumerator UploadMp3AndDownloadMidi(string Mp3FilePath, Button playButton)
    {
        var form = new WWWForm();
        var mp3Bytes = File.ReadAllBytes(Mp3FilePath);

        string mp3FileName = GenerateUniqueFilename(Path.GetFileName(Mp3FilePath));
        ScoreScreen.currentSong = mp3FileName;
        form.AddBinaryData("file", mp3Bytes, mp3FileName, "audio/mpeg");
        
        string mp3Path = Application.persistentDataPath + "/mp3/" + mp3FileName;
        System.IO.File.WriteAllBytes(mp3Path, mp3Bytes);

        mp3Location = mp3Path;
        
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
                string MidiFilePath = Application.persistentDataPath + "/midi/" + Path.GetFileNameWithoutExtension(mp3FileName) + ".mid";
                var midiBytes = request.downloadHandler.data;
                File.WriteAllBytes(MidiFilePath, midiBytes);
                midiLocation = MidiFilePath;
                playButton.gameObject.SetActive(true);
                Destroy(loadingAnimation);
            }
        }
    }


    public const string seperator = "$!&(^$)%!";
    public const string entry_seperator = ")%J)H$%\n";
    
    public void OnSongKeyValueChanged(int index) {
        key = index;
        updatePitchShift();
    }
    
    public void OnDesiredPianoKeyChanged(int index)
    {
        desiredKey = index;
        updatePitchShift();
    }

    private void updatePitchShift()
    {
        int difference = key - desiredKey;

        if (difference > 0)
        {
            if (difference > 6)
            {
                pitchShift = -12 + difference;
            }
            else
            {
                pitchShift = difference;
            }
        }
        else
        {
            if (difference < -6)
            {
                pitchShift = 12 + difference;
            }
            else
            {
                pitchShift = difference;
            }
        }
        

        if (pitchShift >= 0)
        {
            pitchShiftText.text = "Piano Shifting +" + pitchShift;
        }
        else
        {
            pitchShiftText.text = "Piano Shifting " + pitchShift;
        }
        
    }
    
    public void playPressed()
    {
        if (!string.IsNullOrEmpty(song.text) && !string.IsNullOrEmpty(artist.text))
        {
            string combinedTitle = song.text + " - " + artist.text;
            string fileName = "songs.txt"; 
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            
            string textToWrite = entry_seperator + combinedTitle + seperator + mp3Location + seperator + midiLocation + seperator + pitchShift + seperator;
            
            //current game
            
            StreamWriter writer = new StreamWriter(filePath, true);
            
            writer.WriteLine(textToWrite);
            writer.Close();
            
            StartCoroutine(GameManager.GetSongAudioClipThenStartGame(mp3Location, midiLocation, pitchShift));
        }
    }
}
