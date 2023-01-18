
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class midi : MonoBehaviour
{
    public AudioPlayer player;

    private List<int> _noteValues = new List<int>();
    private List<float> _noteTimes = new List<float>();

    private int currentNote = 0;
    private AudioSource mp3Player;
    private const float BPM = 120;
    
    //make a class for midi files. have notes in it, and have function to generate lead melody (highest notes, and make it singable, so not super frequent notes.
    //i can use pausing for the audio clip to wait for the player to sing the note before moving to the next. find vocal range and select notes within there.
    public void Start()
    {
        var midiFile = MidiFile.Read("Assets/hi.mid");
        int timeDiv = ((TicksPerQuarterNoteTimeDivision) midiFile.TimeDivision).TicksPerQuarterNote;

        foreach (var note in midiFile.GetNotes())
        {
            long beatNumber = note.Time / timeDiv;
            float beatTime = beatNumber * (60f / BPM);
            _noteValues.Add(note.NoteNumber);
            _noteTimes.Add(beatTime);
            // StartCoroutine(player.PlayNoteAfterSeconds(60 - note.NoteNumber, beatTime));
        }
        mp3Player = GetComponent<AudioSource>();
        mp3Player.Play();
        // StartCoroutine(PlayNoteAfterSeconds(_noteValues[currentNote], _noteTimes[currentNote]));
    }
    
    private void Update()
    {
        if (mp3Player.time >= _noteTimes[currentNote])
        {
            if (_noteValues[currentNote] > 50)
            {
                Debug.Log(_noteValues[currentNote]);
                // player.PlayNote(_noteValues[currentNote] - 60);
            }
            currentNote += 1;
        }
    }

    // public IEnumerator PlayNoteAfterSeconds(int midiNumber, float seconds)
    // {
    //     yield return new WaitForSeconds(seconds);
    //     Debug.Log(mp3Player.time - seconds);
    //     player.PlayNote(2);
    // }

}
