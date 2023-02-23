using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public Runway2 runway;
    private AudioSource _micSource;
    private PitchDetector _detector;
    private const float PitchOf0 = 130.815f;
    
    public GameObject vertex;
    public GameObject moving;

    [FormerlySerializedAs("pianoAudioPlayer")] public MidiPlayer pianoMidiPlayer;
    
    //use .enabled

    public int currentNote = -1;
    public int correctNoteValue;
    
    void Start()
    {
        Time.fixedDeltaTime = 0.01f;
        
        _micSource = GetComponent<AudioSource>();
        
        //test if this actually works by filtering at 20k hertz and making sure no signal is detected
        
        _micSource.clip = Microphone.Start(Microphone.devices[0], true, 100, AudioSettings.outputSampleRate);

        _detector = new PitchDetector(130f, 300f, Time.fixedDeltaTime, 0.05f, 0.1f, 2f, _micSource.clip);
        Invoke("assignLoudness", 0.1f);
    }

    public void ListenForNote(int midiValue)
    {
        currentNote = -1;
        correctNoteValue = midiValue;
    }
    
    public void StopListening()
    {
        // if (currentNote == -1)
        runway.CorrectNoteMissed();
    }

    private void assignLoudness()
    {
        _detector.GetNoInputVolumeLoudness(0.1f);
    }

    void FixedUpdate()
    {
        try
        {
            int position = Microphone.GetPosition(Microphone.devices[0]);
            float pitch = _detector.GetPitchAtSamplePosition(position);
            
            int newNote = MovePlayerXAndReturnNote(pitch);
            
            //see if the note changed
            if (newNote != currentNote)
            {
                if (newNote == correctNoteValue)
                {
                    runway.CorrectNoteHit();
                }
                else
                {
                    runway.CorrectNoteMissed();
                }
            }

            currentNote = newNote;
        }
        catch (ArgumentException){}
    }

    int MovePlayerXAndReturnNote(float pitch)
    {
        var position = transform.position;
        float x = PitchToX(pitch);
        
        transform.position = new Vector3(x, position.y, position.z);
        
        return Mathf.RoundToInt(x) + 48;
    }
    
    private static float PitchToX(float pitch)
    {
        return 12 * (Mathf.Log(pitch, 2) - Mathf.Log(PitchOf0, 2));
    }
    
    
}
