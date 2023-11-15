using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldGenerator : MonoBehaviour, IDamageable
{
    public void Init(VoidGameObjectDelegate _destroyCallback)
    {
        destroyCallback = _destroyCallback;
        curHp = maxHp;
    }

    public float GetCurHp => curHp;

    public void GetDamage(float _dmg)
    {
        if (curHp < 0)
            return;

        curHp -= _dmg;

        if (curHp < 0)
        {
            destroyCallback?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    private VoidGameObjectDelegate destroyCallback = null;

    [SerializeField]
    private float curHp = 0;
    [SerializeField]
    private float maxHp = 0;
}
