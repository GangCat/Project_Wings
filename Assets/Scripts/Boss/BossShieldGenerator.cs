using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldGenerator : MonoBehaviour
{
    public void Init(VoidGameObjectDelegate _destroyCallback)
    {
        destroyCallback = _destroyCallback;
    }

    private void OnTriggerEnter(Collider _other)
    {
        destroyCallback?.Invoke(gameObject);
        Destroy(gameObject);
    }

    private VoidGameObjectDelegate destroyCallback = null;
}
