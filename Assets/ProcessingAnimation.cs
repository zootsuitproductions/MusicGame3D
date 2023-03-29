using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProcessingAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private string[] states = new[] { "Processing", "Processing.", "Processing..", "Processing..." };

    private int currentState = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoToNextAnimState());
    }

    private IEnumerator GoToNextAnimState()
    {
        yield return new WaitForSeconds(0.5f);
        currentState += 1;
        text.text = states[currentState % states.Length];
        StartCoroutine(GoToNextAnimState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
