using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MelodyRunway : MonoBehaviour
{
    // public bool waitModeActivated = false; //if true, the game will wait for the user to sing the proper note before moving on each time
    //
    // public GameObject notePrefab; // the object to represent a note on the piano roll
    // public float timeScale = 1; // the scale factor of 1 time unit to 1 world unit
    //
    // private MidiData _data; // object representing a midi file
    //
    // private List<Note> _notes;
    //
    // private List<GameObject> _noteObjects;
    // private GameObject combinedNotes;
    //
    // private AudioSource _audioSource;
    //
    // public PlayerMovement playerMovement;
    //
    // public ScoreBoard scoreBoard;
    //
    // private int pitchShift = 0;
    //
    //
    // //spawn notes in only within a certain distance
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     _data = new MidiData("Assets/Midi/bach.mid", pitchShift, 48, 60, false, 2.5f);
    //     _notes = _data.GetMelodyNotes();
    //     _noteObjects = new List<GameObject>();
    //     _audioSource = GetComponent<AudioSource>();
    //     _audioSource.Play();
    //     InstantiateNotes();
    // }
    //
    // //Instantiates physical notes on the piano roll
    // void InstantiateNotes()
    // {
    //     combinedNotes = new GameObject();
    //     combinedNotes.transform.SetParent(this.transform);
    //     combinedNotes.transform.localPosition = Vector3.zero;
    //
    //     Debug.Log("notes count: " + _notes.Count);
    //     for (int i = 0; i < _notes.Count; i++)
    //     {
    //         Note note = _notes[i];
    //         
    //         //CHANGE THIS. MIN VOC RANGE IS NOT NECESARILY C
    //         
    //         
    //         
    //        
    //         
    //         //!
    //         
    //         int x = (note.Value) - 48;//% 12;
    //         float z = note.Time * timeScale;
    //         
    //         GameObject noteObject = Instantiate(notePrefab, combinedNotes.transform);
    //         noteObject.name = x.ToString();
    //         noteObject.transform.localPosition = new Vector3(x, 1, z);
    //         _noteObjects.Add(noteObject);
    //     }
    // }
    //
    // //how soon you hit the note affects point score.
    //
    // void MoveNotes()
    // {
    //     combinedNotes.transform.localPosition = new Vector3(0, 0, -1 * _audioSource.time * timeScale);
    // }
    //
    // private bool paused = false;
    //
    // private void Pause()
    // {
    //     _audioSource.Pause();
    //     timePaused = Time.time;
    //     _audioSource.time = _data.GetCurrentNote().Time;
    //     paused = true;
    // }
    //
    // public void Unpause()
    // {
    //     _audioSource.UnPause();
    //     paused = false;
    // }
    //
    // private float micLookaheadTime = 0.1f;
    // // remove note from note array when its been hit by the user.
    // //check if note is there when 
    //
    // //make sure there aren't two of same notes in a row in the midi!!!
    // //!!!!!!!
    //
    //
    //
    // //audio not playing now
    // private float timePaused = -1f;
    // // Update is called once per frame
    // void Update()
    // {
    //     Note currentMidiNote = _data.GetCurrentNote();
    //
    //     if (!paused)
    //     {
    //         MoveNotes();
    //         if (_audioSource.time >= currentMidiNote.Time - 2 * micLookaheadTime)
    //         {
    //             StopAllCoroutines();
    //             StartCoroutine(FadeAudioSource.StartFade(_audioSource, 0.02f, 0f));
    //         }
    //         
    //         if (_audioSource.time >= currentMidiNote.Time - micLookaheadTime)
    //         {
    //             playerMovement.micActive = true;
    //         }
    //
    //         if (_audioSource.time >= currentMidiNote.Time + 0.5f)
    //         {
    //             scoreBoard.Increment(false);
    //             Pause();
    //         }
    //     }
    //     
    //     if ((playerMovement.currentNote - currentMidiNote.Value) % 12 == 0
    //         && _audioSource.time >= currentMidiNote.Time - 0.01f 
    //         && _audioSource.time <= currentMidiNote.EndTime)
    //     {
    //         playerMovement.micActive = false;
    //         Destroy(_noteObjects[_data.GetCurrentNoteIndex()]);
    //         
    //         StopAllCoroutines();
    //         StartCoroutine(FadeAudioSource.StartFade(_audioSource, 0.02f, 1f));
    //         if (paused)
    //         {
    //             scoreBoard.UpdateScore(Time.time - timePaused);
    //             Unpause();
    //         }
    //         else
    //         {
    //             scoreBoard.UpdateScore(_audioSource.time - currentMidiNote.Time);
    //             scoreBoard.Increment(true);
    //         }
    //         _data.GoToNextNote();
    //         
    //     }
    //     
    //     //calculate midi note in terms of users vocal range first, and then compare exact values
    // }
}
