using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PianoRoll : MonoBehaviour
{
    [SerializeField] private SongPlayer songPlayer;
    [SerializeField] private PlayerMovement2D playerMovement;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject exampleNotePrefab;
    [SerializeField] private Scoreboard scoreboard;
    [SerializeField] private GameManager _gameManager;
    
    private GameObject _currentNoteObject;
    private MidiData _midiData;
    private Note _currentNote;

    private bool notePrompted = false;
    private bool currentNoteIsExampleNote = true;

    private float _timeInAdvanceToPromptNote = 1.5f;

    public void InitializeNotes(string midiFilePath, int pitchShift, int minPossibleMidiNote, int maxPossibleMidiNote, float minTimeBetweenMelodyNotes)
    {
        _midiData = new MidiData(midiFilePath,
            pitchShift, minPossibleMidiNote, maxPossibleMidiNote, new int[]{}, 
            minTimeBetweenMelodyNotes, minTimeBetweenMelodyNotes);
        
        _currentNote = _midiData.GetNextMelodyNote(songPlayer.GetPlaybackPosition() + 1,false);
    }
    
    //NEED A FUNCTION TO ENSURE THE NOTES HAVE ENOUGH TIME TO HAPPEN WITHOUT PROBLEMS. and the next note should be next note after a time. 

    // Update is called once per frame
    void Update()
    {
        if (!notePrompted)
        {
            //do the note in advance a bit
            if (songPlayer.GetPlaybackPosition() >= _currentNote.Time - _timeInAdvanceToPromptNote)
            {
                PromptNote(_currentNote, currentNoteIsExampleNote);
            }
        }
    }

    public void GoToNextNote(bool repeatValue)
    {
        scoreboard.Increment(_currentNote.Value, !repeatValue);
        
        //this won't work right now if note missed. well it will, but the times will be wrong
        _currentNote = _midiData.GetNextMelodyNote(songPlayer.GetPlaybackPosition() + 1, repeatValue);

        if (_currentNote == null)
        {
            _gameManager.EndGame();
        }
        
        notePrompted = false;
    }

    void PromptNote(Note note, bool isExampleNote)
    {
        if (isExampleNote)
        {
            _currentNoteObject = Instantiate(exampleNotePrefab, transform);
            _currentNoteObject.GetComponent<ExampleNoteMovement>().Instantiate(note, songPlayer, playerMovement, this, _timeInAdvanceToPromptNote);
            currentNoteIsExampleNote = false;
        }
        else
        {
            _currentNoteObject = Instantiate(notePrefab, transform);
            _currentNoteObject.GetComponent<NoteMovement>().Instantiate(note, songPlayer, playerMovement, this, _timeInAdvanceToPromptNote);

        }
        
        notePrompted = true;
    }
}
