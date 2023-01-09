using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private static AudioSource _source;
    public AudioClip[] noteArray;
    
    private static  AudioClip[] _noteArray2;
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _noteArray2 = noteArray;
    }

    public static void PlayNote(int midiNumber)
    {
        _source.clip = _noteArray2[midiNumber];
        _source.Play();
    }
}
