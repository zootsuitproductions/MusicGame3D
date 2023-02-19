using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public MelodyRunway runway;
    private AudioSource _source;
    private PitchDetector _detector;
    private const float PitchOf0 = 130.815f;
    
    public GameObject vertex;
    public GameObject moving;

    [FormerlySerializedAs("pianoAudioPlayer")] public MidiPlayer pianoMidiPlayer;


    public int currentNote = -1; 
    
    void Start()
    {
        Time.fixedDeltaTime = 0.01f;
        
        _source = GetComponent<AudioSource>();
        
        //test if this actually works by filtering at 20k hertz and making sure no signal is detected
        
        _source.clip = Microphone.Start(Microphone.devices[0], true, 100, AudioSettings.outputSampleRate);

        _detector = new PitchDetector(130f, 300f, Time.fixedDeltaTime, 0.05f, 0.1f, 2f, _source.clip);
        Invoke("assignLoudness", 0.1f);
    }

    private void assignLoudness()
    {
        _detector.GetNoInputVolumeLoudness(0.1f);
    }

    public bool micActive = false; 

    // Update is called once per frame
    void FixedUpdate()
    {
        if (micActive)
        {
            // Vector3 pos = moving.transform.position;
            // moving.transform.position = new Vector3(pos.x, pos.y, -2*Time.time);
            try
            {
                int position = Microphone.GetPosition(Microphone.devices[0]);
            
                float pitch = _detector.GetPitchAtSamplePosition(position);
                // GameObject obj = Instantiate(vertex, new Vector3(PitchToX(posi), 1, 0), Quaternion.identity);
                // obj.transform.parent = moving.transform;
                MovePlayerX(pitch);
            }
            catch (ArgumentException)
            {
                // QuantizeX();
            } 
        }
        
    }

    private void QuantizeX()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(Mathf.RoundToInt(pos.x), pos.y, pos.z);
    }
    //
    // private void OnCollisionEnter(Collision collision)
    // {
    //     GameObject obj = collision.gameObject;
    //     int midiNum = int.Parse(obj.name);
    //     pianoAudioPlayer.PlayNote(midiNum);
    //     Destroy(obj);
    //     runway.Unpause();
    // }

    void MovePlayerX(float pitch)
    {
        var position = transform.position;
        float x = PitchToX(pitch);

        currentNote = Mathf.RoundToInt(x) + 48; //midi note
        transform.position = new Vector3(x, position.y, position.z);
    }
    
    private static float PitchToX(float pitch)
    {
        return 12 * (Mathf.Log(pitch, 2) - Mathf.Log(PitchOf0, 2));
    }
    
    
}
