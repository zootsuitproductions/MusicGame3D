using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPlayerMovement : MonoBehaviour
{
    public Camera cam;
    public MidiPlayer midiPlayer;
    
    public int currentNote = -1;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit  hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                  
            if (Physics.Raycast(ray, out hit))
            {
                if (int.TryParse(hit.transform.name, out int parsed))
                {
                    var position = transform.position;
                    currentNote = parsed;
                    midiPlayer.PlayNote(currentNote);
                    transform.position = new Vector3(parsed, position.y, position.z);
                }
                
            }
        }
    }
}
