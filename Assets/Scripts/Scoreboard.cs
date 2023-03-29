using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    private TMP_Text correct;

    private int numCorrect = 0;
    private int total = 0;

    public static float score = 0;
    public static int totalNotes = 0;

    private Dictionary<int, int> _individualNoteCorrectNumbers = new Dictionary<int, int>();
    private Dictionary<int, int> _individualNoteTotalOccurrences = new Dictionary<int, int>();
    
    //keep track of individual note accuracy on each note (got 3/4 G's right, 0/2 F's, etc)
    private void AddOneToKey(Dictionary<int, int> dict, int key)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] += 1;
        } 
        else
        {
            dict[key] = 1;
        }
    }
    
    public void Increment(int noteVal, bool correct)
    {
        if (correct)
        {
            AddOneToKey(_individualNoteCorrectNumbers, noteVal);
            numCorrect += 1;
        }
        
        AddOneToKey(_individualNoteTotalOccurrences, noteVal);
        total += 1;
        totalNotes = total;
        score = (float) numCorrect / (float) total;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        score = 0;
        correct = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        correct.text = "Correct " + numCorrect + "/" + total;
    }


}
