using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScore;

    
    
    private static float highScoreValue = 0;
    public static string currentSong = "";

    // Start is called before the first frame update
    void Start()
    {
        if (Scoreboard.totalNotes < 10)
        {
            scoreText.text = "You didn't play long enough to get scored!";
        }
        else
        {
            UpdateHighScoreIfNecessary(Scoreboard.score);
            scoreText.text = "Score: " + Scoreboard.score.ToString("0.00");
            
        }
        highScoreValue = PlayerPrefs.GetFloat(currentSong, 0);
        highScore.text = "High Score: " + highScoreValue.ToString("0.00");
    }

    public static void UpdateHighScoreIfNecessary(float score)
    {
        
        if (score > highScoreValue)
        {
            highScoreValue = Scoreboard.score;
            PlayerPrefs.SetFloat(currentSong, highScoreValue);
        }
    }
}