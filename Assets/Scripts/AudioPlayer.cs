using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private static AudioSource _source;
    public AudioClip[] noteArray;
    
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayNote(int midiNumber)
    {
        _source.clip = noteArray[midiNumber];
        _source.Play();
    }
}
