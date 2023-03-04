using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string songAudioFilePath;
    [SerializeField] private string midiFilePath;
    
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
        this.songAudioFilePath = songAudioFilePath;
        this.midiFilePath = midiFilePath;
        this.minTimeBetweenMelodyNotes = minTimeBetweenMelodyNotes;
        this.minPossibleMidiNote = minPossibleMidiNote;
        this.maxPossibleMidiNote = maxPossibleMidiNote;
        this.pitchShift = pitchShift;
    }
    
    void Start()
    {
        songPlayer.LoadSong(songAudioFilePath);
        playerMovement2D.SetPitchRange(minPossibleMidiNote, maxPossibleMidiNote);
        playerMovement2D.SetPitchShift(pitchShift);
        
        pianoKeyColor.SetMidiNoteHighlighted(0);
        //change color of root notes
        pianoRoll.InitializeNotes(midiFilePath, pitchShift, minPossibleMidiNote, maxPossibleMidiNote, minTimeBetweenMelodyNotes);
    }
}
