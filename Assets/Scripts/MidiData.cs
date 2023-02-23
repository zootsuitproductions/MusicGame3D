using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class MidiData
{

    private List<Note> _notes = new List<Note>();
    private List<Note> _melodyNotes = new List<Note>();
    private const float BPM = 120;

    private int minNote;
    private int maxNote;

    private float threshold;
    private float _wrongNoteCooldown;

    private int[] _blacklisted;
    
    // Create a representation of a melody from a midi file, from a midi file name to read from,
    // an integer number of notes to pitch up by, and the minimum and maximum notes of the range of the outputted melody.
    public MidiData(String filename, int pitchShift, int minMidiNote, int maxMidiNote, int[] blacklistedNotesInRange, float sameNoteThresh, float wrongNoteCooldown)
    {
        minNote = minMidiNote;
        maxNote = maxMidiNote;
        
        _blacklisted = blacklistedNotesInRange;


        threshold = sameNoteThresh;
        _wrongNoteCooldown = wrongNoteCooldown;
       
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
        
        // GraphNotes(false);
        //get the timing right for more dispersed notes
        
        
        //make the threshold dynamic as time goes on. increasing difficulty. find out the minimum possible given the 
        // mic limitations
        // _melodyNotes = GetMelody(_notes, sameNoteThresh, leaveUnclamped);

        // Note nextNote = GetFirstMelodyNote(sameNoteThresh);
        // while (nextNote != null)
        // {
        //     _melodyNotes.Add(nextNote);
        //     nextNote = GetNextMelodyNoteAny(nextNote, sameNoteThresh);
        // }
        //
        // _melodyNotes = Clamp(_melodyNotes);
    }

    public Note GetNextMelodyNote(bool repeatValue)
    {
        if (_currentNoteIndex == 0)
            return ClampNoteToVocalRange(GetFirstMelodyNote(threshold));

        if (!repeatValue)
        {
            return ClampNoteToVocalRange(GetFirstDifferentNoteAfterThreshold(_notes[_currentNoteIndex], threshold));
        }
        else
        {
            return ClampNoteToVocalRange(GetSameNoteInTimeRange(_notes[_currentNoteIndex], threshold, 2 * _wrongNoteCooldown));
        }
    }


    private int _currentNoteIndex = 0;

    private Note GetFirstNoteInRange()
    {
        for (int i = 0; i < _notes.Count; i++)
        {
            Note potentialNote = _notes[i];
            if (NoteIsWithinRange(potentialNote))
            {
                _currentNoteIndex = i;
                return potentialNote;
            }
        }

        throw new ArgumentException("No first note in range.");
    }
    private Note GetFirstMelodyNote(float threshold)
    {
        Note note = GetFirstNoteInRange();
        for (int i = _currentNoteIndex; i < _notes.Count; i++)
        {
            Note potentialNote = _notes[i];
            if (NoteIsWithinRange(potentialNote))
            {
                if (potentialNote.Time - note.Time > threshold)
                    return note;

                if (potentialNote.Value > note.Value)
                {
                    _currentNoteIndex = i;
                    note = potentialNote;
                }
                   
            }
        }

        throw new ArgumentException("No valid first note detected");
    }

    private Note GetSameNoteInTimeRange(Note currentNote, float minTime, float maxTime)
    {
        int savedNoteIndex = _currentNoteIndex;
        
        for (int i = _currentNoteIndex; i < _notes.Count; i++)
        {
            Note potentialNote = _notes[i];

            if (potentialNote.Time - currentNote.Time >= maxTime)
            {
                break;
            }
            
            // make sure note is within range
            if (!NoteIsWithinRange(potentialNote)) continue;
            
            // ensure that threshold seconds have passed before the potential note
            if (potentialNote.Time - currentNote.Time < minTime) continue;
            
            // ensure clamped notes are the same
            if (ClampNoteToVocalRange(potentialNote).Value 
                != ClampNoteToVocalRange(currentNote).Value) continue;
            
            _currentNoteIndex = i;
            return potentialNote;
        }

        _currentNoteIndex = savedNoteIndex;
        return GetFirstDifferentNoteAfterThreshold(currentNote, minTime);
    }
    
    // get the first note of a different midi value that occurs after a certain number of seconds
    private Note GetFirstDifferentNoteAfterThreshold(Note currentNote, float threshold)
    {
        for (int i = _currentNoteIndex; i < _notes.Count; i++)
        {
            Note potentialNote = _notes[i];
            
            // make sure note is within range
            if (!NoteIsWithinRange(potentialNote)) continue;
            
            // ensure that threshold seconds have passed before the potential note
            if (potentialNote.Time - currentNote.Time < threshold) continue;
            
            // ensure clamped notes aren't the same
            if (ClampNoteToVocalRange(potentialNote).Value 
                == ClampNoteToVocalRange(currentNote).Value) continue;

            _currentNoteIndex = i;
            return potentialNote;
        }

        return null;
    }

    private bool NoteIsWithinRange(Note potentialNote)
    {
        return (potentialNote.Value > 36 && IsClampedNoteInVocalRange(potentialNote));
    }
    
    [CanBeNull]
    private Note GetNextMelodyNoteAny(Note currentNote, float threshold)
    {
        Note note = GetFirstDifferentNoteAfterThreshold(currentNote, threshold);

        if (note == null)
            return null;
        
        for (int i = _currentNoteIndex + 1; i < _notes.Count; i++)
        {
            Note potentialNote = _notes[i];

            // no more simultaneous notes, so stick with the last one saved to note var
            if (potentialNote.Time - note.Time >= threshold) break;

            if (!NoteIsWithinRange(potentialNote)) continue;
            if (potentialNote.Value <= note.Value) continue;
            if (ClampNoteToVocalRange(potentialNote).Value 
                == ClampNoteToVocalRange(currentNote).Value) continue;
            
            note = potentialNote;
            _currentNoteIndex = i;

        }
        
        PopulateOctaveNoteDic(note);
        return note;
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
    
    public List<Note> GetAllNotes()
    {
        List<Note> newList = new List<Note>(_notes.Count);

        _notes.ForEach((item) =>
        {
            newList.Add((Note)item.Clone());
        });
        
        return newList;
    }
    
    public List<Note> GetMelodyNotes()
    {
        List<Note> newList = new List<Note>(_notes.Count);

        _melodyNotes.ForEach((item) =>
        {
            newList.Add((Note)item.Clone());
        });
        
        return newList;
    }

    private bool IsClampedNoteInVocalRange(Note note)
    {
        int clampedVal = ClampNoteToVocalRange(note).Value;

        foreach (var val in _blacklisted)
        {
            if (val == clampedVal)
                return false;
        }
        
        return clampedVal <= maxNote && clampedVal >= minNote;
    }

    private List<Note> Clamp(List<Note> notes) {
        
        //keeps track of the actual melody notes
        List<Note> clamped = new List<Note>();
        for (int i = 0; i < notes.Count; i++)
        {
            PopulateOctaveNoteDic(notes[i]);
        }

        for (int i = 0; i < notes.Count; i++)
        {
            clamped.Add( ClampNoteToVocalRange(notes[i]));
        }

        return clamped;
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
}
