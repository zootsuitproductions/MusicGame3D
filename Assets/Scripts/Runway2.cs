using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runway2 : MonoBehaviour
{
    private int _pitchShift = 0;
    private MidiData _data; // object representing a midi file
    private List<Note> _melodyNotes;
    private List<Note> _allNotes;

    private AudioSource _audioSource;
    private float _musicVolume = 1f;
    
    public MidiPlayer midiPlayer;
    private const float FADE_TIME = 0.05f; //the time to fade out the song audio
    
    public ScoreBoard scoreBoard;
    
    private Note _currentNote;
    
    private GameObject combinedNotes;
    
    //player state
    public PlayerMovement playerMovement;
    
    public GameObject notePrefab;
    public float timeScale = 1;

    private float _sameNoteThresh = 2.5f;

    private float _wrongNoteCooldown = 6f;

    private GameObject _currentNoteObject;
    
    private const float NOTE_WINDOW_OPEN_TIME = 0.1f;
    private const float NOTE_WINDOW_CLOSE_TIME = 1f;

    public bool headphones_mode = true;
    private bool fadedOut = false;

    // Start is called before the first frame update
    void Start()
    {
        //difficulty slider should be how fast the note is shown versus when you have to sing it. instead
        // of always sliding in from a safe distance
        
        //show tonic in a different color on piano?
        
        //also revealing the tonic note should cost something in the game
        
        
        _data = new MidiData("Assets/Midi/bach.mid", _pitchShift, 48, 52, new int[]{}, _sameNoteThresh, _wrongNoteCooldown);
        
        //haave setting: doesPitchShiftAffectAudio -- choose whether or not to shift the audio and the mic input as well,
        // i . e, just be playing song with c major scale even though its in a different key
        
        _audioSource = GetComponent<AudioSource>();
        _currentNote = _data.GetNextMelodyNote(false);
        InstantiateNotesParent();

        InstantiateNote(_currentNote);
        _audioSource.Play();

        playerMovement.enabled = false;
    }

    void InstantiateNote(Note note)
    {
        Destroy(_currentNoteObject);
        int x = (note.Value) - 48;//% 12;
        float z = note.Time * timeScale;
            
        _currentNoteObject = Instantiate(notePrefab, combinedNotes.transform);
        _currentNoteObject.name = x.ToString();
        _currentNoteObject.transform.localPosition = new Vector3(x, 1, z);
    }

    //Instantiates physical notes on the piano roll
    void InstantiateNotesParent()
    {
        combinedNotes = new GameObject();
        combinedNotes.transform.SetParent(this.transform);
        combinedNotes.transform.localPosition = Vector3.zero;
    }
    
    void MoveNotes()
    {
        combinedNotes.transform.localPosition = new Vector3(0, 0, -1 * _audioSource.time * timeScale);
    }

    public void CorrectNoteHit()
    {
        playerMovement.enabled = false;
        if (!headphones_mode)
        {
            fadedOut = false;
            StartCoroutine(FadeIn(_audioSource, FADE_TIME));
        }
        
        midiPlayer.PlayNote(_currentNote.Value);
        scoreBoard.Increment(_currentNote.Value, true);
        
        _currentNote = _data.GetNextMelodyNote(false);
        InstantiateNote(_currentNote);
    }
    
    public void CorrectNoteMissed()
    {
        playerMovement.enabled = false;
        if (!headphones_mode)
        {
            fadedOut = false;
            StartCoroutine(FadeIn(_audioSource, FADE_TIME));
        }
        
        scoreBoard.Increment(_currentNote.Value, false);
        
        _currentNote = _data.GetNextMelodyNote(true);
        InstantiateNote(_currentNote);
    }

    public IEnumerator FadeOutThenEnableMic(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, 0f, currentTime / duration);
            yield return null;
        }

        playerMovement.enabled = true;
        yield break;
    }
    public IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, _musicVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    private bool WithinCurrentNoteWindow()
    {
        return (_audioSource.time >= _currentNote.Time - NOTE_WINDOW_OPEN_TIME
                && _audioSource.time <= _currentNote.Time + NOTE_WINDOW_CLOSE_TIME);
    }

    private bool AfterNoteWindow()
    {
        return _audioSource.time > _currentNote.Time + NOTE_WINDOW_CLOSE_TIME;
    }
    
    void Update()
    {
        MoveNotes();

        if (WithinCurrentNoteWindow())
        {
            if (!headphones_mode && !fadedOut)
            {
                fadedOut = true;
                StartCoroutine(FadeOutThenEnableMic(_audioSource, FADE_TIME));
            }
            playerMovement.enabled = true;
            playerMovement.ListenForNote(_currentNote.Value);
        }
        else if (AfterNoteWindow())
        {
            playerMovement.enabled = false;
            playerMovement.StopListening();
        }

    }

}
