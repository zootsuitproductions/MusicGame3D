using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MP3Uploader : MonoBehaviour
{
    public void ConvertMp3ToMidi(string Mp3FilePath, Button playButton)
    {
        StartCoroutine(UploadMp3AndDownloadMidi(Mp3FilePath, playButton));
    }

    private IEnumerator UploadMp3AndDownloadMidi(string Mp3FilePath, Button playButton)
    {
        var form = new WWWForm();
        var mp3Bytes = File.ReadAllBytes(Mp3FilePath);
        form.AddBinaryData("Mp3FieldName", mp3Bytes, Path.GetFileName(Mp3FilePath), "audio/mpeg");

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
                Debug.Log("MIDI file saved to: {MidiFilePath}");
                
                playButton.gameObject.SetActive(true);
            }
        }
    }
    
    // public static IEnumerator UploadMP3(string mp3FilePath, Button playButton)
    // {
    //     // create a new WWWForm
    //     // WWWForm form = new WWWForm();
    //     //
    //     // // read the mp3 file into a byte array
    //     // byte[] mp3Bytes = File.ReadAllBytes(mp3FilePath);
    //     //
    //     // // get the file name without the path
    //     string fileName = Path.GetFileNameWithoutExtension(mp3FilePath);
    //     //
    //     // //save to persistent data
    //     // // string mp3Path = Application.persistentDataPath + "/mp3/" + fileName + ".mp3";
    //     // // System.IO.File.WriteAllBytes(mp3Path, mp3Bytes);
    //     //
    //     // // NewFile.mp3Location = mp3Path;
    //     //
    //     // // add the mp3 file to the form data
    //     // form.AddBinaryData(fileName, mp3Bytes, fileName + ".mp3", "audio/mpeg");
    //
    //     // send a POST request to the server
    //     
    //     var form = new WWWForm();
    //     var mp3Bytes = File.ReadAllBytes(mp3FilePath);
    //     form.AddBinaryData(fileName, mp3Bytes, Path.GetFileName(mp3FilePath), "audio/mpeg");
    //
    //     
    //     using (UnityWebRequest www = UnityWebRequest.Post("https://dannysantanasf.pythonanywhere.com/upload", form))
    //     {
    //         byte[] mp3Bytes = File.ReadAllBytes(mp3FilePath);
    //         www.uploadHandler = new UploadHandlerRaw(mp3Bytes);
    //         www.SetRequestHeader("Content-Type", "audio/mpeg");
    //
    //         yield return www.SendWebRequest();
    //
    //         // check for errors
    //         if (www.result != UnityWebRequest.Result.Success)
    //         {
    //             Debug.LogError(www.error);
    //         }
    //         else
    //         {
    //             var midiBytes = UnityWebRequest.downloadHandler.data;
    //             File.WriteAllBytes(MidiFilePath, midiBytes);
    //             
    //             Debug.Log("Upload complete! " + fileName);
    //
    //             // create a MemoryStream to write the MIDI data to
    //             MemoryStream midiStream = new MemoryStream();
    //
    //             // create a BinaryWriter to write to the MemoryStream
    //             BinaryWriter midiWriter = new BinaryWriter(midiStream);
    //
    //             // write the MIDI data to the MemoryStream using the BinaryWriter
    //             midiWriter.Write(www.downloadHandler.data);
    //
    //             // save the MemoryStream to disk as a binary file
    //             string filePath = Application.persistentDataPath + "/midi/" + fileName + ".mid";
    //             FileStream midiFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
    //             midiStream.WriteTo(midiFile);
    //             midiFile.Close();
    //             midiStream.Close();
    //
    //             Debug.Log("MIDI file saved to: " + filePath);
    //
    //             NewFile.midiLocation = filePath;
    //             playButton.gameObject.SetActive(true);
    //         }
    //
    //     }
    // }
}