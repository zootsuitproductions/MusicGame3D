using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static AudioClip songClip;
    private static string midiFilePath;
    
    [SerializeField] private float minTimeBetweenMelodyNotes;
    [SerializeField] private int minPossibleMidiNote;
    [SerializeField] private int maxPossibleMidiNote;
    [SerializeField] private int pitchShift;
    
    [SerializeField] private SongPlayer songPlayer;
    [SerializeField] private PlayerMovement2D playerMovement2D;
    [SerializeField] private PianoRoll pianoRoll;
    [SerializeField] private PianoKeyColor pianoKeyColor;

    public void Initialize(string songAudioFilePath, string midiFilePath, float minTimeBetweenMelodyNotes, int minPossibleMidiNote, int maxPossibleMidiNote, int pitchShift)
    {
        this.minTimeBetweenMelodyNotes = minTimeBetweenMelodyNotes;
        this.minPossibleMidiNote = minPossibleMidiNote;
        this.maxPossibleMidiNote = maxPossibleMidiNote;
        this.pitchShift = pitchShift;
    }

    public static void StartGame(AudioClip songClip, string midiPath)
    {
        GameManager.songClip = songClip;
        GameManager.midiFilePath = midiPath;
        SceneManager.LoadScene("Game");
    }
    
    public static IEnumerator GetSongAudioClipThenStartGame(string mp3Path, string midiPath)
    {
        string webFilePath = "file://" + mp3Path;
        
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(webFilePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                StartGame(clip, midiPath);
            }
        }
    }

    void StartGame()
    {
        songPlayer.LoadSong(songClip);
        playerMovement2D.SetPitchRange(minPossibleMidiNote, maxPossibleMidiNote);
        playerMovement2D.SetPitchShift(pitchShift);
        
        pianoKeyColor.SetMidiNoteHighlighted(0);
        //change color of root notes
        pianoRoll.InitializeNotes(midiFilePath, pitchShift, minPossibleMidiNote, maxPossibleMidiNote, minTimeBetweenMelodyNotes);
    }

    void Start()
    {
        StartGame();
    }
}
