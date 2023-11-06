using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHoming : MonoBehaviour
{
    private void Awake()
    {
        spawnTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - spawnTime < 1)
            return;
        Destroy(gameObject);
    }

    private float spawnTime = 0f;
}
