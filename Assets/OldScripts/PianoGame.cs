using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PianoGame : MonoBehaviour
{
    
    // private int currentNote = 0;
    // private int previousNote = 0;
    //
    // private Renderer[] keyRenderers;
    // private Random rand = new Random();
    //
    // public Material selected;
    // private int[] noteChoices;
    //
    // public Transform player;
    //
    // private AudioPlayer _audioPlayer;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     _audioPlayer = GetComponent<AudioPlayer>();
    //     noteChoices = new[] { 0, 2, 4, 5, 7, 9, 11, 12 };
    //     keyRenderers = gameObject.GetComponentsInChildren<Renderer>();
    //     // keyRenderers[0].enabled = false;
    //     UpdateCurrentNote();
    // }
    //
    // private Material previousMaterial;
    // void UpdateCurrentNote()
    // {
    //     currentNote = noteChoices[rand.Next(0, noteChoices.Length)];
    //     if (currentNote == previousNote)
    //     {
    //         UpdateCurrentNote();
    //         return;
    //     }
    //     
    //     previousMaterial = keyRenderers[currentNote].material;
    //     keyRenderers[currentNote].material = selected;
    //     keyRenderers[previousNote].material = previousMaterial;
    //
    //     previousNote = currentNote;
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     if (Mathf.Abs(player.position.x - (float)currentNote) < 0.1f) {
    //         _audioPlayer.PlayNote(currentNote);
    //         UpdateCurrentNote();
    //     }
    // }
}
