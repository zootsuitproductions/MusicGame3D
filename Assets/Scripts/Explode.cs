using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private GameObject[] shards;
    [SerializeField] private float destroyTime;
    void Start()
    {
        foreach (var shard in shards)
        {
            Rigidbody2D rb = shard.GetComponent<Rigidbody2D>();
            rb.AddForce(shard.transform.localPosition * 0.5f, ForceMode2D.Impulse);
        }

        StartCoroutine(DelayedDestroy(destroyTime));
    }

    IEnumerator DelayedDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
