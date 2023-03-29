using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleNoteMovement : NoteMovement
{

    [SerializeField] private ExampleNoteSoundController _soundController;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private float timeToPlayAudioAfterShattering = 1f;

    // Update is called once per frame
    void Update()
    {
        if (!_spriteRenderer.enabled)
        {
            timeToPlayAudioAfterShattering -= Time.deltaTime;
            
            if (timeToPlayAudioAfterShattering <= 0)
                Destroy(gameObject);
            return;
        }
        
        if (_songPlayer.GetPlaybackPosition() <= _note.Time)
        {
            MoveYValueAndReturnTrueWhenDone(initialPositionY, finalPositionY, _speed);
        }
        else if (_songPlayer.GetPlaybackPosition() >= _note.Time + 1f)
        {
            //1 second after the time of the note
            _pianoRoll.GoToNextNote(false);
            Shatter(transform.localScale);
            _spriteRenderer.enabled = false;
        }
        else
        {
            ShakeNote();
            _soundController.playMidiNoteOnce(_note.Value);
            //at position, shake for 1 second and get bigger
            transform.localScale += new Vector3(Time.deltaTime,Time.deltaTime,Time.deltaTime)/4;
        }
        

    }
}
