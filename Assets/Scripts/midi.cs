
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class midi : MonoBehaviour
{
    public AudioPlayer player;
    private List<Note> notes = new List<Note>();

    private const float BPM = 120;
    public void Start()
    {
        var midiFile = MidiFile.Read("Assets/hi.mid");
        int timeDiv = ((TicksPerQuarterNoteTimeDivision) midiFile.TimeDivision).TicksPerQuarterNote;
        // Debug.Log(midiFile.GetTempoMap().ti);
        //
        foreach (var note in midiFile.GetNotes())
        {
            // Debug.Log(note.Time);
            notes.Add(note);
            // TimeDivision
            long beatNumber = note.Time / timeDiv;
            float beatTime = beatNumber * (60f / BPM);

            StartCoroutine(player.PlayNoteAfterSeconds(60 - note.NoteNumber, beatTime));
        }
    }
    
}
