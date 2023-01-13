using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private AudioSource _source;
    private PitchDetector _detector;
    private const float PitchOf0 = 130.815f;

    public AudioPlayer pianoAudioPlayer;
    
    void Start()
    {
        Time.fixedDeltaTime = 0.03f;
        
        _source = GetComponent<AudioSource>();
        _source.clip = Microphone.Start(Microphone.devices[0], true, 100, AudioSettings.outputSampleRate);
        _detector = new PitchDetector(125f, 500f, 0.2f, _source.clip);
    }

    private float lastXPosition = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        try
        {
            int position = Microphone.GetPosition(Microphone.devices[0]);
            
            MovePlayerX(_detector.GetPitchAtSamplePosition(position));
        }
        catch (ArgumentException)
        {
            QuantizeX();
        }
    }

    private void QuantizeX()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(Mathf.RoundToInt(pos.x), pos.y, pos.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        int midiNum = int.Parse(obj.name);
        pianoAudioPlayer.PlayNote(midiNum);
        Destroy(obj);
    }

    void MovePlayerX(float pitch)
    {
        var position = transform.position;
        float x = PitchToX(pitch);
        
        if (Mathf.Abs(x - lastXPosition) <= 0.2f)
        {
            lastXPosition = x;
            transform.position = new Vector3(x, position.y, position.z);
        }
        
        lastXPosition = x;
    }
    
    private static float PitchToX(float pitch)
    {
        return 12 * (Mathf.Log(pitch, 2) - Mathf.Log(PitchOf0, 2));
    }
    
    
}
