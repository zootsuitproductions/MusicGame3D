using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class MyMidiFile
{
    private List<int> _noteValues = new List<int>();
    private List<float> _noteTimes = new List<float>();

    private const float BPM = 120;
    
    public MyMidiFile(String filename)
    {
        var midiFile = MidiFile.Read(filename);
        int timePerPeat = ((TicksPerQuarterNoteTimeDivision) midiFile.TimeDivision).TicksPerQuarterNote;

        foreach (var note in midiFile.GetNotes())
        {
            long beatNumber = note.Time / timePerPeat;
            float beatTime = beatNumber * (60f / BPM);
            _noteValues.Add(note.NoteNumber);
            _noteTimes.Add(beatTime);
        }
    }
    
    
    
    
}
