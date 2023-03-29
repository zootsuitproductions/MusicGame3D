using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleNoteSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip[] notes;

    private bool alreadyPlayed = false;
    private AudioSource source;
    
    public void playMidiNoteOnce(int midiNumber)
    {
        if (!alreadyPlayed)
        {
            source.clip = GetClipOfMidiNoteNumber(midiNumber);
            source.Play();

            alreadyPlayed = true;
        }
    }

    private AudioClip GetClipOfMidiNoteNumber(int midiNum)
    {
        foreach (var note in notes)
        {
            int noteNum = int.Parse(note.name);
            if (midiNum - noteNum <= 6 && midiNum - noteNum >= -6)
            {
                float difference = midiNum - noteNum;
                source.pitch = Mathf.Pow(2, (difference / 12f));
                return note;
            }
        }
        return notes[1];
    }
    
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
}
