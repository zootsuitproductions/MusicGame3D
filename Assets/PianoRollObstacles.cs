using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoRollObstacles : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    private float speed = 7f;
    private Transform[] currentRows;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject row = new GameObject("row");
        currentRows = new Transform[1];
        GameObject gameObject = Instantiate(obstaclePrefabs[0], row.transform);
        row.transform.position = new Vector3(0, 1, 100);
        currentRows[0] = row.transform;
    }

    // Update is called once per frame
    void Update()
    {
        currentRows[0].Translate(new Vector3(0, 0, -1 * speed * Time.deltaTime));
    }
}
