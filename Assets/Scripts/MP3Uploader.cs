using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

public class MP3Uploader : MonoBehaviour
{
    public static IEnumerator UploadMP3(string mp3FilePath)
    {
        // create a new WWWForm
        WWWForm form = new WWWForm();

        // read the mp3 file into a byte array
        byte[] mp3Bytes = File.ReadAllBytes(mp3FilePath);
        
        // get the file name without the path
        string fileName = Path.GetFileNameWithoutExtension(mp3FilePath);

        // add the mp3 file to the form data
        form.AddBinaryData("file", mp3Bytes, "myFile.mp3", "audio/mpeg");

        // send a POST request to the server
        using (UnityWebRequest www = UnityWebRequest.Post("https://dannysantanasf.pythonanywhere.com/upload", form))
        {
            yield return www.SendWebRequest();

            // check for errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Upload complete!");
                byte[] midiData = www.downloadHandler.data;
                string filePath = Application.persistentDataPath + "/midi/" + fileName + ".mid";
                System.IO.File.WriteAllBytes(filePath, midiData);
                Debug.Log("MIDI file saved to: " + filePath);
                
            }
        }
    }
}