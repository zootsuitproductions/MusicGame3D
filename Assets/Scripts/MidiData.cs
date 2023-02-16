using System;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class MidiData
{

    private List<Note> _notes = new List<Note>();
    private const float BPM = 120;
    
    private int currentNote = 0;

    private int minNote;
    private int maxNote;
    
    
    private float reqNoteSeperation = 0f;
    
    // Create a representation of a melody from a midi file, from a midi file name to read from,
    // an integer number of notes to pitch up by, and the minimum and maximum notes of the range of the outputted melody.
    public MidiData(String filename, int pitchShift, int minMidiNote, int maxMidiNote)
    {
        minNote = minMidiNote;
        maxNote = maxMidiNote;
       
        var midiFile = MidiFile.Read(filename);
        int timePerPeat = ((TicksPerQuarterNoteTimeDivision) midiFile.TimeDivision).TicksPerQuarterNote;

        // add notes from midi file into our array
        foreach (var note in midiFile.GetNotes())
        {
            float beatNumber = (float) note.Time / timePerPeat;
            float beatNumberEnd = (float) note.EndTime / timePerPeat;
            float startTime = beatNumber * (60f / BPM);
            float endTime = beatNumberEnd * (60f / BPM);

            _notes.Add(new Note(note.NoteNumber + pitchShift, startTime, endTime, note.Velocity));
        }
        
        GraphNotes(false);
        //get the timing right for more dispersed notes
        
        
        //make the threshold dynamic as time goes on. increasing difficulty. find out the minimum possible given the 
        // mic limitations
        _notes = GetMelody(_notes, 1f);
        GraphNotes(true);
    }
    
    //check if a note is represented across multiple octaves in the vocal range
    private bool NoteAppearsMultTimesInVoxRange(Note note)
    {
        int noteMod = note.Value % 12;
        int minMod = minNote % 12;

        return (noteMod >= minMod && noteMod + 12 <= minMod + (maxNote - minNote));
    }
    
    // for notes where the octave is in range, keep track of their exact note values in the original
    // midi file to determine which notes to put higher in the clamped melody
    private Dictionary<int, List<int>> _octaveNoteDictionary = new Dictionary<int, List<int>>();

    private void PopulateOctaveNoteDic(Note note)
    {
        if (NoteAppearsMultTimesInVoxRange(note))
        {
            if (_octaveNoteDictionary.ContainsKey(note.Value % 12))
            {
                if (!_octaveNoteDictionary[note.Value % 12].Contains(note.Value))
                {
                    _octaveNoteDictionary[note.Value % 12].Add(note.Value);
                }
            }
            else
            {
                List<int> list = new List<int>();
                list.Add(note.Value);
                _octaveNoteDictionary[note.Value % 12] = list;
            }
        }
    }
    
    //assign the note to a value within the vocal range
    private Note ClampNoteToVocalRange(Note note)
    {
        int clampedVal = note.Value;
        while (clampedVal > maxNote)
        {
            clampedVal -= 12;
        }

        while (clampedVal < minNote)
        {
            clampedVal += 12;
        }

        if (_octaveNoteDictionary.ContainsKey(note.Value % 12))
        {
            List<int> list = _octaveNoteDictionary[note.Value % 12];
            if (list[0] == note.Value)
            {
                clampedVal -= 12;
            }
        }

        //look at note dic to decide

        return new Note(clampedVal, note.Time, note.EndTime, note.Velocity);
    }
    
    public List<Note> GetNotes()
    {
        List<Note> newList = new List<Note>(_notes.Count);

        _notes.ForEach((item) =>
        {
            newList.Add((Note)item.Clone());
        });
        
        return newList;
    }

    private bool IsClampedNoteInVocalRange(Note note)
    {
        int clampedVal = ClampNoteToVocalRange(note).Value;
        return clampedVal <= maxNote && clampedVal >= minNote;
    }
    private bool IsClampedNoteDifferentThanLastMelodyNote(Note note, List<Note> melody)
    {
       
        return melody.Count == 0 || ClampNoteToVocalRange(melody[^1]).Value != ClampNoteToVocalRange(note).Value;
        
        
    }
    private bool IsClampedNoteDifferentThanSecondToLastMelodyNote(Note note, List<Note> melody)
    {
        return melody.Count < 2 || (ClampNoteToVocalRange(melody[^2]).Value != ClampNoteToVocalRange(note).Value);
    }

    private bool EnoughTimeSinceLastNote(Note note, List<Note> melody)
    {
        return (melody.Count == 0 || note.Time - reqNoteSeperation >= melody[^1].Time);
    }
    private List<Note> GetMelody(List<Note> notes, float threshold) {
        
        //keeps track of the actual melody notes
        List<Note> melody = new List<Note>();
        
        for (int i = 0; i < notes.Count; i++)
        {
            Note potentialNote = notes[i];
            PopulateOctaveNoteDic(potentialNote);

            //don't allow extreme bass notes to factor in, nor notes that don't fit in vocal range
            if (potentialNote.Value <= 36 || !IsClampedNoteInVocalRange(potentialNote))
            {
                continue;
            }
            
            if (melody.Count > 0) {
                Note prevMelodyNote = melody[^1];
                
                if (potentialNote.Time - prevMelodyNote.Time < threshold) {
           
                    if (potentialNote.Value > prevMelodyNote.Value) {
                        
                        //make sure that replacing the last note won't result in a duplicate
                        if (IsClampedNoteDifferentThanSecondToLastMelodyNote(potentialNote, melody))
                        {
                            melody[^1]= potentialNote;
                        }
                    }
                    //don't add this note
                    continue;
                }
            }
            
            //add note to the melody if it's not a duplicate
            if (EnoughTimeSinceLastNote(potentialNote, melody) && IsClampedNoteDifferentThanLastMelodyNote(potentialNote, melody)) {
                melody.Add(potentialNote);
            }
        }

        //clamp all notes in melody to fit the vocal range
        for (int i = 0; i < melody.Count; i++)
        {
            melody[i] = ClampNoteToVocalRange(melody[i]);
        }
        return melody;
    }

    public void GraphNotes(bool red)
    {
        for (int i = 0; i < _notes.Count; i++)
        {
            Note note = _notes[i];
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (red) 
                cube.GetComponent<MeshRenderer>().material.color = Color.red;
            cube.transform.position = new Vector3(note.Time+20f, 5, note.Value);
        }
    }

    public Note GetCurrentNote()
    {
        return _notes[currentNote];
    }

    public int GetCurrentNoteIndex()
    {
        return currentNote;
    }
    

    public void GoToNextNote()
    {
        currentNote += 1;
    }
 
    
}
