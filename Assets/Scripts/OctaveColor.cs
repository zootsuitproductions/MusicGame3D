using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctaveColor : MonoBehaviour
{
    [SerializeField] private GameObject[] keys = new GameObject[12];

    public void SetKeyHighlighted(int keyNumber)
    {
        keys[keyNumber].GetComponent<SpriteRenderer>().color = new Color(0,205,0);
    }
}
