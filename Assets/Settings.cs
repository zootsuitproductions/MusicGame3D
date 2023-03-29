using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool paused = false;

    public void Pause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            
        }
        else
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        paused = !paused;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
