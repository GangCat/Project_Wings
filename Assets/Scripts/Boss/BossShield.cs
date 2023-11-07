using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour, IDamageable
{
    public float GetCurHp => 99999;

    public void GetDamage(float _dmg)
    {
        Debug.Log($"Shield Hit!");
    }
}
