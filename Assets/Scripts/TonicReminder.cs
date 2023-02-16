using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonicReminder : MonoBehaviour
{
    private AudioPlayer _audioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            _audioPlayer.PlayNote(0);
        }
    }
}
