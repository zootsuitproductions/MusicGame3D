using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileBrowser : MonoBehaviour
{
    public void OpenFile()
    {
        string filePath = UnityEditor.EditorUtility.OpenFilePanel("Open File", "", "mp3");
        
        if (!string.IsNullOrEmpty(filePath))
        {
            NewFile.filePath = filePath;
            
            SceneManager.LoadScene("New File Screen");
            // Use the file path to load the file into your game
        }
    }
}
