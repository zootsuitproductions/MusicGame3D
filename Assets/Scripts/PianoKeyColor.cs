using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyColor : MonoBehaviour
{
    [SerializeField] private OctaveColor[] octaves;

    public void SetMidiNoteHighlighted(int midiNote)
    {
        foreach (var octave in octaves)
        {
            octave.SetKeyHighlighted(midiNote % 12);
        }
    }
}
