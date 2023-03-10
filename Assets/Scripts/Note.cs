using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    public int Value { get; set; }
    public float Time { get; }
    public float EndTime { get; }
    public int Velocity { get; }
        
    public Note(int value, float time, float endTime, int velocity)
    {
        Value = value;
        Time = time;
        EndTime = endTime;
        Velocity = velocity;
    }

    public Note Clone()
    {
        return new Note(Value, Time, EndTime, Velocity);
    }

    public static float HertzToMidiNoteValue(float hertz)
    {
        // MIDI note value for A4 (440 Hz)
        const float A4MidiNoteValue = 69f;
    
        // Calculate the MIDI note value based on the formula:
        // MIDI note value = 12 * log2(frequency / 440) + 69
        return 12 * Mathf.Log(hertz / 440f, 2) + A4MidiNoteValue;
    }
    
    public static float MidiNoteToHertz(int note)
    {
        float a = 440f; // frequency of A4 note in hertz
        float power = (note - 69.0f) / 12.0f;
        return a * Mathf.Pow(2, power);
    }
        
}
