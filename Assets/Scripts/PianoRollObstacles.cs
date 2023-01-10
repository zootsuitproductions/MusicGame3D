using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PianoRollObstacles : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    private float speed = 7f;
    private int widthOfTrack = 13;

    private bool[] scaleArray = new[]
        { true, false, true, false, true, true, false, true, false, true, false, true, true };
    
    private List<GameObject> currentRows = new List<GameObject>();
    private Random rand = new Random();
    
    private float lastRowTime = 0f;
    private float timeBetweenNewRows = 1f;

    private float _offScreenZValue = -5f;

    private float startingPosition = 20f;
    private List<int> indexChoices = new List<int>();
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            if (scaleArray[i])
            {
                indexChoices.Add(i);   
            }
        }
        GameObject row = CreateRow(GenerateRandomRowData(1), startingPosition);
        currentRows.Add(row);
    }

    bool[] GenerateRandomRowData(int rowNumber)
    {
        // bool[] arr = new bool[widthOfTrack];
        //
        // bool isPassable = false;
        //
        // for (int i = 0; i < widthOfTrack; i++)
        // {
        //     // int randTopBound = (20 / (rowNumber / 5));
        //     if (!scaleArray[i] || rand.Next(0, 3) == 0)
        //     {
        //         
        //         arr[i] = true;
        //     }
        //     else
        //     {
        //         arr[i] = false;
        //         isPassable = true; 
        //     }
        // }
        //
        // if (!isPassable)
        // {
        //     arr[rand.Next(0, widthOfTrack)] = false;
        // }
        //
        // return arr;
        
        bool[] arr = new bool[widthOfTrack];
        
        for (int i = 0; i < widthOfTrack; i++)
        {
            arr[i] = false;
        }
        
        arr[indexChoices[rand.Next(0, indexChoices.Count)]] = true;

        return arr;
    }

    GameObject CreateRow(bool[] rowData, float zPosition)
    {
        GameObject row = new GameObject("row");
        for (int i = 0; i < rowData.Length; i++)
        {
            if (rowData[i])
            {
                GameObject obstacle = Instantiate(obstaclePrefabs[rand.Next(0, obstaclePrefabs.Length)], row.transform);
                obstacle.name = i.ToString();
                obstacle.transform.localPosition = new Vector3(i, 0, 0);
            }
        }

        row.transform.position = new Vector3(0, 1, zPosition);
        return row;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastRowTime >= timeBetweenNewRows)
        {
            GameObject row = CreateRow(GenerateRandomRowData(1), startingPosition);
            currentRows.Add(row);
            lastRowTime = Time.time;
        }
        

        for (int i = 0; i < currentRows.Count; i++)
        {
            currentRows[i].transform.Translate(new Vector3(0, 0, -1 * speed * Time.deltaTime));
            
            if (currentRows[i].transform.position.z <= _offScreenZValue)
            {
                Destroy(currentRows[i]);
                currentRows.RemoveAt(i);
            }
        }
    }
}
