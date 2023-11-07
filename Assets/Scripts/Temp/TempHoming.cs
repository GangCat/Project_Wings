using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHoming : AttackableObject
{
    private void Awake()
    {
        spawnTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
            return;

        if (AttackDmg(other))
        {
            if (isFirstTrigger)
            {
                isFirstTrigger = false;
                return;
            }
            else
                Destroy(gameObject);
        }
    }

    private float spawnTime = 0f;
}
