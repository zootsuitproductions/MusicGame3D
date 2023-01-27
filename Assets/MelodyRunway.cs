using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyRunway : MonoBehaviour
{
    public GameObject notePrefab;
    public float timeScale = 1;

    private MidiReader _reader;

    private List<Note> _notes;

    private List<GameObject> _noteObjects;
    private GameObject combinedNotes;

    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {   
        _reader = new MidiReader("Assets/hi.mid");
        _notes = _reader.GetNotes();
        _noteObjects = new List<GameObject>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
        InstantiateNotes();
    }

    //Instantiates physical notes on the piano roll
    void InstantiateNotes()
    {
        combinedNotes = new GameObject();
        combinedNotes.transform.SetParent(this.transform);
        combinedNotes.transform.localPosition = Vector3.zero;
        
        
        for (int i = 0; i < _notes.Count; i++)
        {
            Note note = _notes[i];
            int x = (note.Value-2) % 12;
            float z = note.Time * timeScale;
            
            GameObject noteObject = Instantiate(notePrefab, combinedNotes.transform);
            noteObject.name = x.ToString();
            noteObject.transform.localPosition = new Vector3(x, 1, z);
            _noteObjects.Add(noteObject);
        }
    }
    
    void MoveNotes()
    {;
        combinedNotes.transform.localPosition = new Vector3(0, 0, -1 * _audioSource.time * timeScale);
    }

    private bool paused = false;

    private void Pause()
    {
        _audioSource.Pause();
        paused = true;
    }
    
    public void Unpause()
    {
        _audioSource.Play();
        paused = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            MoveNotes();
            int? note = _reader.GetNoteAtTime(_audioSource.time);
            if (note != null)
            {
                Pause();
            }
        }
    }
}
