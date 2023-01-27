using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class MidiReader
{

    private List<Note> _notes = new List<Note>();
    private const float BPM = 120;

    private int currentNote = 0;
    
    public MidiReader(String filename)
    {
        var midiFile = MidiFile.Read(filename);
        int timePerPeat = ((TicksPerQuarterNoteTimeDivision) midiFile.TimeDivision).TicksPerQuarterNote;

        foreach (var note in midiFile.GetNotes())
        {
            float beatNumber = (float) note.Time / timePerPeat;
            float beatNumberEnd = (float) note.EndTime / timePerPeat;
            float startTime = beatNumber * (60f / BPM);
            float endTime = beatNumberEnd * (60f / BPM);
            
            _notes.Add(new Note(note.NoteNumber, startTime, endTime, note.Velocity));
        }

        _notes = GetMelody(_notes, 1.5f);
        //clean up the messy notes
    }

    public List<Note> GetNotes()
    {
        return this._notes;
    }
    
    private List<Note> GetMelody(List<Note> notes, float threshold) {
        List<Note> melody = new List<Note>();
        for (int i = 0; i < notes.Count; i++) {
            Note currentNote = notes[i];
            bool addNote = true;
            for (int j = 0; j < melody.Count; j++) {
                Note melodyNote = melody[j];
                if (Mathf.Abs(currentNote.Time - melodyNote.Time) < threshold) {
                    if (currentNote.Value > melodyNote.Value) {
                        melody[j] = currentNote; // replace with higher pitch note
                    }
                    addNote = false;
                    break;
                }
            }
            if (addNote) {
                melody.Add(currentNote);
            }
        }
        return melody;
    }

    public void GraphNotes()
    {
        for (int i = 0; i < _notes.Count; i++)
        {
            Note note = _notes[i];
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(note.Time, 5, note.Value);
        }
    }

    public void removeNonMelodyNotes()
    {
        filterSynchronousNotes(0.1f);
        
        //take into account velocity
        for (int i = 0; i < _notes.Count; i++)
        {
            Debug.Log(_notes[i].Time);
        }
    }
    
    private void filterSynchronousNotes(float durationBetweenNotesThreshold)
    {
        List<Note> newNotes = new List<Note>();
        List<Note> noteCandidates = new List<Note>();
        
        for (int i = 0; i < _notes.Count - 1; i++)
        {
            if (_notes[i].Time + durationBetweenNotesThreshold > _notes[i+1].Time)
            {
                noteCandidates.Add(_notes[i]);
            }
            else
            {
                if (newNotes.Count == 0 || _notes[i].Value != newNotes[newNotes.Count - 1].Value)
                {
                    noteCandidates.Add(_notes[i]);
                }
                
                if (noteCandidates.Count > 0)
                {
                    Note max = noteCandidates[0];
                    //get highest midi note value
                    for (int j = 1; j < noteCandidates.Count; j++)
                    {
                        if (noteCandidates[j].Value > max.Value)
                        {
                            max = noteCandidates[j];
                        }
                    }
                    newNotes.Add(max);
                    noteCandidates.Clear();
                }
                // newNotes.Add(_notes[i]);
            }
            
        }

        _notes = newNotes;
    }

    public int? GetNoteAtTime(float time)
    {
        Note note = _notes[currentNote];
        if (time >= note.Time)
        {
            currentNote += 1;
            return note.Value;
        }
        
        return null;
    }
    
    
    
    
}
