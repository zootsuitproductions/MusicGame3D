
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class midi : MonoBehaviour
{
    public void Start()
    {
        var midiFile = MidiFile.Read("Assets/hi.mid");
        foreach (var note in midiFile.GetNotes())
        {
            Debug.Log(note.Time);
        }
        
    }
}
