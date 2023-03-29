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
    private static int pitchShift;

    [SerializeField] private float minTimeBetweenMelodyNotes;
    [SerializeField] private int minPossibleMidiNote;
    [SerializeField] private int maxPossibleMidiNote;

    [SerializeField] private SongPlayer songPlayer;
    [SerializeField] private PlayerMovement2D playerMovement2D;
    [SerializeField] private PianoRoll pianoRoll;
    [SerializeField] private PianoKeyColor pianoKeyColor;

    public void Initialize(string songAudioFilePath, string midiFilePath, float minTimeBetweenMelodyNotes, int minPossibleMidiNote, int maxPossibleMidiNote, int pitchShift)
    {
        this.minTimeBetweenMelodyNotes = minTimeBetweenMelodyNotes;
        this.minPossibleMidiNote = minPossibleMidiNote;
        this.maxPossibleMidiNote = maxPossibleMidiNote;
    }

    private static void SwitchToGameScene(AudioClip songClip, string midiPath, int pitchShift)
    {
        GameManager.songClip = songClip;
        GameManager.midiFilePath = midiPath;
        GameManager.pitchShift = pitchShift;
        SceneManager.LoadScene("Game");
    }

    public void EndGame()
    {
        SceneManager.LoadScene("High Score");
    }
    
    public static IEnumerator GetSongAudioClipThenStartGame(string mp3Path, string midiPath, int pitchShift)
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
                SwitchToGameScene(clip, midiPath, pitchShift);
            }
        }
    }

    private void StartGame()
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
