using System.Collections;
using UnityEngine;

public class MidiPlayer : MonoBehaviour
{
    public int VOICES = 6;
    private float VOLUME = 0.5f;
    
    private static AudioSource _source;
    public AudioClip[] noteArray;

    private AudioSource[] _sources;
    private int _currentVoice = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _sources = new AudioSource[VOICES];
        for (int i = 0; i < VOICES; i++)
        {
            _sources[i] = gameObject.AddComponent<AudioSource>();
            _sources[i].volume = VOLUME;
        }
    }

    public void PlayNote(int midiNumber)
    {
        _sources[_currentVoice].clip = noteArray[midiNumber%12]; 
        //fix, make all notes
        _sources[_currentVoice].Play();
        _currentVoice = (_currentVoice + 1) % VOICES;
    }

    //
    // public IEnumerator PlayNoteAfterSeconds(int midiNumber, float seconds)
    // {
    //     yield return new WaitForSeconds(seconds);
    //     Debug.Log("playing note at " + seconds);
    //     this.PlayNote(midiNumber);
    // }
}
