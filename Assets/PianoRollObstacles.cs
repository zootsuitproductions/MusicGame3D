using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PianoRollObstacles : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    private float speed = 7f;
    private List<GameObject> currentRows = new List<GameObject>();
    private Random rand = new Random();

    private float _offScreenZValue = -5f;
    
    // Start is called before the first frame update
    void Start()
    {
        // GameObject row = new GameObject("row");
        GameObject row = CreateRow(new[] { true, true, true, true,true, true, true, true,true, true, true, true }, 50f);
        currentRows.Add(row);
        // GameObject gameObject = Instantiate(obstaclePrefabs[0], row.transform);
        // row.transform.position = new Vector3(0, 1, 100);
        // currentRows[0] = row.transform;

    }

    GameObject CreateRow(bool[] rowData, float zPosition)
    {
        GameObject row = new GameObject("row");
        for (int i = 0; i < rowData.Length; i++)
        {
            if (rowData[i])
            {
                GameObject obstacle = Instantiate(obstaclePrefabs[rand.Next(0, obstaclePrefabs.Length)], row.transform);
                obstacle.transform.localPosition = new Vector3(i, 0, 0);
            }
        }

        row.transform.position = new Vector3(0, 1, zPosition);
        return row;
    }
    

    // Update is called once per frame
    void Update()
    {
        currentRows[0].transform.Translate(new Vector3(0, 0, -1 * speed * Time.deltaTime));

        for (int i = 0; i < currentRows.Count; i++)
        {
            if (currentRows[i].transform.position.z <= _offScreenZValue)
            {
                Destroy(currentRows[i]);
                currentRows.RemoveAt(i);
                GameObject row = CreateRow(new[] { true, true, false, true }, 50f);
                currentRows.Add(row);
            }
        }
    }
}
