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

    public void Play(MidiPlayer player)
    {
        player.PlayNote(this.Value);
    }
        
}
