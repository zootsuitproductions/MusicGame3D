using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScrollView : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        string midiPath = Path.Combine(Application.persistentDataPath, "midi");
        Directory.CreateDirectory(midiPath);

        string mp3Path = Path.Combine(Application.persistentDataPath, "mp3");
        Directory.CreateDirectory(mp3Path);
        
        string fileName = "songs.txt";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            // Create a StreamReader instance and open the file for reading
            StreamReader reader = new StreamReader(filePath);

            // Read the entire contents of the file
            string fileContents = reader.ReadToEnd();
            reader.Close();

            string[] songs = fileContents.Split(NewFile.entry_seperator);
            Debug.Log(songs.Length);
            foreach (var song in songs)
            {
                if (!string.IsNullOrEmpty(song))
                {
                    string[] pieces = song.Split(NewFile.seperator);
                    GameObject row = Instantiate(itemPrefab, this.transform);
                    row.GetComponent<ScrollItem>().Initialize(pieces[0], pieces[1], pieces[2]);
                }
            }
        }
    }
}
