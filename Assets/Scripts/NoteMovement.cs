using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NoteMovement : MonoBehaviour
{
    [SerializeField] private float finalPositionY;
    [SerializeField] private float initialPositionY;
    
    [SerializeField] private GameObject shattered;

    private float elapsedTime = 0f;

    private float health = 0.3f;

    private Note _note;
    private float _shakeFrequency;
    private SongPlayer _songPlayer;
    private PlayerMovement2D _playerMovement2D;
    private PianoRoll _pianoRoll;
    private float _speed;
    
    public void Instantiate(Note note, SongPlayer songPlayer, PlayerMovement2D playerMovement2D, PianoRoll pianoRoll, float secondsInAdvanceToStartPromptingNote)
    {
        _note = note;
        _shakeFrequency = Note.MidiNoteToHertz(_note.Value);
        _songPlayer = songPlayer;
        _playerMovement2D = playerMovement2D;
        _pianoRoll = pianoRoll;
        
        _speed = (finalPositionY - initialPositionY) / secondsInAdvanceToStartPromptingNote;

        transform.localPosition = new Vector3(note.Value, initialPositionY, 4);
    }

    private void DecreaseHealthAndCheckIfDead(float decreaseBy)
    {
        health -= decreaseBy;
        ShakeNote();
        
        if (health <= 0f)
        {
            _pianoRoll.GoToNextNote(gameObject, false);
            Shatter();
        }
    }

    private void Shatter()
    {
        Instantiate(shattered, transform.parent).transform.localPosition = transform.localPosition;
        Destroy(gameObject);
    }

    private float _shakeTime = 0f;

    private float _shakeWidthX = 0.1f;

    private void ShakeNote()
    {
        _shakeTime += Time.deltaTime;
        transform.localPosition = new Vector3(_note.Value + _shakeWidthX * (_shakeTime + 1) * Mathf.Sin(_shakeFrequency * 2 * Mathf.PI * _shakeTime), transform.localPosition.y, transform.localPosition.z);
    }

    private bool MoveYValueAndReturnTrueWhenDone(float initialY, float endY, float speed)
    {
        Vector3 pos = transform.localPosition;
        if (pos[1] == endY)
            return true;
        
        if (endY > initialY)  //increasing
        {
            pos[1] += Time.deltaTime * speed;

            if (pos[1] > endY)
                pos[1] = endY;
        }
        else //decreasing
        {
            pos[1] -= Time.deltaTime * speed;
            
            if (pos[1] < endY)
                pos[1] = endY;
        }
        
        transform.localPosition = pos;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_songPlayer.GetPlaybackPosition() < _note.Time + 1f)
        {
            MoveYValueAndReturnTrueWhenDone(initialPositionY, finalPositionY, _speed);
        } 
        else if (MoveYValueAndReturnTrueWhenDone(finalPositionY, initialPositionY, _speed)) {
            //mind the GAP ??!!
            _pianoRoll.GoToNextNote(gameObject, true);
        }
        
        if (Mathf.Abs(_playerMovement2D.transform.localPosition.x - transform.localPosition.x) < 0.4f)
        {
            DecreaseHealthAndCheckIfDead(Time.deltaTime);
        }
        
    }
}
    

