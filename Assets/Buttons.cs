using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPlayGame()
    {
        SceneManager.LoadScene("New Game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
