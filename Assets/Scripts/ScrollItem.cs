using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private int pitchShift;
    
    public void Initialize(string songAndArtist, string mp3Path, string midiPath, int pitchShift)
    {
        song.text = songAndArtist;
        this.midiPath = midiPath;
        this.pitchShift = pitchShift;
        this.mp3Path = mp3Path;
    }

    public void Play()
    {
        ScoreScreen.currentSong = Path.GetFileName(mp3Path);
        StartCoroutine(GameManager.GetSongAudioClipThenStartGame(mp3Path, midiPath, pitchShift));
    }
}
