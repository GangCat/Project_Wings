using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPoint : MonoBehaviour
{
    public void Init(VoidGameObjectDelegate _destroyCallback)
    {
        gameObject.layer = LayerMask.NameToLayer("BossWeakPoint");
        destroyCallback = _destroyCallback;
    }

    private void OnTriggerEnter(Collider _other)
    {
        destroyCallback?.Invoke(gameObject);
        Destroy(gameObject);
    }

    private VoidGameObjectDelegate destroyCallback = null;
}
