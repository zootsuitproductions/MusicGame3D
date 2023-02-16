using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    private TMP_Text scoreText;
    private TMP_Text correct;

    private int numCorrect = 0;
    private int total = 0;

    private int score = 0;
    
    //keep track of individual note accuracy on each note (got 3/4 G's right, 0/2 F's, etc)

    public void Increment(bool correct)
    {
        if (correct)
        {
            numCorrect += 1;
        }

        total += 1;
    }
    
    //perfect timing should give 100 points

    public void UpdateScore(float lateTime)
    {
        score += 100 - (int) Mathf.Abs(20*lateTime);
    }
    
    public void UpdateIncorrectNote(int noteDifference)
    {
        
    }
    // Start is called before the first frame update
    void Awake()
    {
        TMP_Text[] components = GetComponentsInChildren<TMP_Text>();
        Debug.Log(components.Length);
        scoreText = components[1];
        correct = components[0];

    }

    // Update is called once per frame
    void Update()
    {
        correct.text = "Correct " + numCorrect + "/" + total;
        scoreText.text = "Score: " + score;
    }
    
    
}
