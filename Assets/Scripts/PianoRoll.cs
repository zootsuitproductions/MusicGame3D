using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PianoRoll : MonoBehaviour
{
    [SerializeField] private SongPlayer songPlayer;
    [SerializeField] private PlayerMovement2D playerMovement;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Scoreboard scoreboard;
    
    private GameObject _currentNoteObject;
    private MidiData _midiData;
    private Note _currentNote;

    private bool notePrompted = false;

    private float _timeInAdvanceToPromptNote = 1.5f;

    public void InitializeNotes(string midiFilePath, int pitchShift, int minPossibleMidiNote, int maxPossibleMidiNote, float minTimeBetweenMelodyNotes)
    {
        _midiData = new MidiData("Assets/Resources/" + midiFilePath + ".mid", 
            pitchShift, minPossibleMidiNote, maxPossibleMidiNote, new int[]{}, 
            minTimeBetweenMelodyNotes, minTimeBetweenMelodyNotes);
        
        _currentNote = _midiData.GetNextMelodyNote(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!notePrompted)
        {
            //do the note in advance a bit
            if (songPlayer.GetPlaybackPosition() >= _currentNote.Time - _timeInAdvanceToPromptNote)
            {
                PromptNote(_currentNote);
            }
        }
       
    }

    public void GoToNextNote(GameObject currentNoteObject, bool repeatValue)
    {
        scoreboard.Increment(_currentNote.Value, !repeatValue);
        
        //this won't work right now if note missed. well it will, but the times will be wrong
        _currentNote = _midiData.GetNextMelodyNote(repeatValue);
        
        notePrompted = false;
        Destroy(currentNoteObject);
    }

    
    void PromptNote(Note note)
    {
        _currentNoteObject = Instantiate(notePrefab, transform);
        _currentNoteObject.GetComponent<NoteMovement>().Instantiate(note, songPlayer, playerMovement, this, _timeInAdvanceToPromptNote);
        
        notePrompted = true;
    }
}
