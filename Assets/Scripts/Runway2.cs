using System.Collections.Generic;
using UnityEngine;

public class Runway2 : MonoBehaviour
{
    private int _pitchShift = 0;
    private MidiData _data; // object representing a midi file
    private List<Note> _melodyNotes;
    private List<Note> _allNotes;


    private AudioSource _audioSource;
    public MidiPlayer midiPlayer;
    private const float FADE_TIME = 0.2f; //the time to fade out the song audio
    
    public ScoreBoard scoreBoard;
    
    //state variables
    private bool _puased = false; //either PLAYING or PAUSED
    private int _currentNoteIndex = 0; //the current midi note the player has to identify
    private Note _currentNote;
    
    private GameObject combinedNotes;
    
    //player state
    public PlayerMovement playerMovement;
    
    public GameObject notePrefab;
    public float timeScale = 1;

    private float _sameNoteThresh = 2.5f;

    private float _wrongNoteCooldown = 6f;

    // Start is called before the first frame update
    void Start()
    {
        _data = new MidiData("Assets/Midi/bach.mid", _pitchShift, 48, 60, false, _sameNoteThresh, _wrongNoteCooldown);
        // _melodyNotes = _data.GetMelodyNotes();
        _audioSource = GetComponent<AudioSource>();
        _currentNote = _data.GetNextMelodyNote(false);
        InstantiateNotes();

        InstantiateNote(_currentNote);
        _audioSource.Play();
    }

    void InstantiateNote(Note note)
    {
        int x = (note.Value) - 48;//% 12;
        float z = note.Time * timeScale;
            
        GameObject noteObject = Instantiate(notePrefab, combinedNotes.transform);
        noteObject.name = x.ToString();
        noteObject.transform.localPosition = new Vector3(x, 1, z);
    }

    //Instantiates physical notes on the piano roll
    void InstantiateNotes()
    {
        combinedNotes = new GameObject();
        combinedNotes.transform.SetParent(this.transform);
        combinedNotes.transform.localPosition = Vector3.zero;
        
        // for (int i = 0; i < _melodyNotes.Count; i++)
        // {
        //     Note note = _melodyNotes[i];
        //
        //     int x = (note.Value) - 48;//% 12;
        //     float z = note.Time * timeScale;
        //     
        //     GameObject noteObject = Instantiate(notePrefab, combinedNotes.transform);
        //     noteObject.name = x.ToString();
        //     noteObject.transform.localPosition = new Vector3(x, 1, z);
        // }
    }
    
    void MoveNotes()
    {
        combinedNotes.transform.localPosition = new Vector3(0, 0, -1 * _audioSource.time * timeScale);
    }

    // private void Pause()
    // {
    //     state = State.PAUSED;
    // }
    //
    // public void Unpause()
    // {
    //     StartCoroutine(FadeAudioSource.FadeIn(_audioSource, FADE_TIME));
    //     state = State.PLAYING;
    // }
    //
    //
    // void UpdateNextNote()
    // {
    //     if (_audioSource.time >= _currentNote.Time + ALLOWED_LATE_TIME)
    //         StartCoroutine(FadeAudioSource.FadeOut(_audioSource, FADE_TIME, midiPlayer));
    //
    //     //INST$EAD OF PAUSING, LOOK AHEAD IN MIDI FOR THE SAME NOTE,
    //     //AND MAKE THAT THE NEXT ONE THEY HAVE TO PLAY
    // }
    
    //get the melody notes one at a time. Run the algorithm to get the
    // next melody note based on the current one. then we dont need all notes on screen at once.

    void Update()
    {
        MoveNotes();
        if (Input.GetKeyDown(KeyCode.A))
        {
            InstantiateNote(_data.GetNextMelodyNote(false));
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            InstantiateNote(_data.GetNextMelodyNote(true));
        }
        // if (!_puased)
        // {
        //     MoveNotes();
        //     PauseIfNeccessary();
        // }
    }

}



// using System.Collections.Generic;
// using UnityEngine;
//
// public class MidiInput : MonoBehaviour
// {
//     private int _pitchShift = -2;
//     private MidiData _data; // object representing a midi file
//     private List<Note> _notes;
//
//
//     private AudioSource _audioSource;
//     public MidiPlayer midiPlayer;
//     private const float FADE_TIME = 0.2f; //the time to fade out the song audio
//     
//     public ScoreBoard scoreBoard;
//     
//     //state variables
//     private enum State { PLAYING, PAUSED}
//     private State state = State.PLAYING; //either PLAYING or PAUSED
//     private int currentNote = 0; //the current midi note the player has to identify
//     
//     //player state
//     public ClickPlayerMovement playerMovement;
//
//     // Start is called before the first frame update
//     void Start()
//     {   
//         //DONT NEED VOCAL RANGE THING. THAT SHOULD BE SEPARATE FUNCTION IN MIDI PARSER
//         _data = new MidiData("Assets/Midi/satie.mid", _pitchShift, 48, 60, true, 2f);
//         _notes = _data.GetNotes();
//         _audioSource = GetComponent<AudioSource>();
//         _audioSource.pitch = 0.891f;
//         _audioSource.Play();
//     }
//
//     private void PauseSongAndPlayNote()
//     {
//         StartCoroutine(FadeAudioSource.FadeOutThenPlayNote(_audioSource, FADE_TIME, midiPlayer, _notes[this.currentNote]));
//         state = State.PAUSED;
//     }
//     
//     public void Unpause()
//     {
//         StartCoroutine(FadeAudioSource.FadeIn(_audioSource, FADE_TIME));
//         state = State.PLAYING;
//     }
//
//     void PlayCurrentNoteIfTimeReached()
//     {
//         Note currentNote = _notes[this.currentNote];
//
//         if (_audioSource.time >= currentNote.Time - FADE_TIME)
//         {
//             PauseSongAndPlayNote();
//         }
//         
//         //do melodic phrases instead of single notes
//     }
//
//     void ContinueIfPlayerOnRightNote()
//     {
//         if (playerMovement.currentNote == _notes[this.currentNote].Value % 12)
//         {
//             currentNote += 1;
//             Unpause();
//         }
//     }
//     
//     void Update()
//     {
//         switch (state)
//         {
//             case State.PLAYING:
//                 PlayCurrentNoteIfTimeReached();
//                 break;
//             case State.PAUSED:
//                 ContinueIfPlayerOnRightNote();
//                 break;
//         }
//     }
// }
//
