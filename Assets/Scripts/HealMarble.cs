using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMarble : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
            _other.GetComponent<StatusHp>().HealHp(healAmount);
    }

    [SerializeField]
    private float healAmount = 30f;
}
