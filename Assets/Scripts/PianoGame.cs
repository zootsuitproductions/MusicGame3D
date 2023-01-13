using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PianoGame : MonoBehaviour
{
    private int currentNote = 0;
    private int previousNote = 0;

    private Renderer[] keyRenderers;
    private Random rand = new Random();

    private int[] noteChoices;
    // Start is called before the first frame update
    void Start()
    {
        noteChoices = new[] { 0, 2, 4, 5, 7, 9, 11, 12 };
        keyRenderers = gameObject.GetComponentsInChildren<Renderer>();
        // keyRenderers[0].enabled = false;
        UpdateCurrentNote();
    }

    private Material previousMaterial;
    void UpdateCurrentNote()
    {
        currentNote = noteChoices[rand.Next(0, noteChoices.Length)];
        if (currentNote == previousNote)
        {
            UpdateCurrentNote();
            return;
        }
        
        previousMaterial = keyRenderers[currentNote].material;
        keyRenderers[currentNote].material.SetColor("_Color", Color.green);
        keyRenderers[previousNote].material = previousMaterial;

        previousNote = currentNote;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
