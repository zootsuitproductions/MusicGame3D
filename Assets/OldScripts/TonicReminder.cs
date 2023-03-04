using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonicReminder : MonoBehaviour
{
    private MidiPlayer _midiPlayer;
    // Start is called before the first frame update
    void Start()
    {
        _midiPlayer = GetComponent<MidiPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            _midiPlayer.PlayNote(0);
        }
    }
}
