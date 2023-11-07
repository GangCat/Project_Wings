using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldGenerator : MonoBehaviour, IDamageable
{
    public void Init(VoidGameObjectDelegate _destroyCallback)
    {
        destroyCallback = _destroyCallback;
    }

    public float GetCurHp => 1f;

    public void GetDamage(float _dmg)
    {
        destroyCallback?.Invoke(gameObject);
        Destroy(gameObject);
    }

    private VoidGameObjectDelegate destroyCallback = null;

}
