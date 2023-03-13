using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollItem : MonoBehaviour
{
    [SerializeField] private TMP_Text song;
    [SerializeField] private Button button;

    private string mp3Path;
    private string midiPath;
    
    public void Initialize(string songAndArtist, string mp3Path, string midiPath)
    {
        song.text = songAndArtist;
        this.midiPath = midiPath;
        this.mp3Path = mp3Path;
    }

    public void Play()
    {
        StartCoroutine(GameManager.GetSongAudioClipThenStartGame(mp3Path, midiPath));
    }
}
