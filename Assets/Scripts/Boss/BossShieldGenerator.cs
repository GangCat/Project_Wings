using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BossShieldGenerator : MonoBehaviour, IDamageable
{
    public void Init(VoidGameObjectDelegate _destroyCallback, Vector3 _bossPos)
    {
        destroyCallback = _destroyCallback;
        curHp = maxHp;

        StartCoroutine(GenIndicatorCoroutine(_bossPos));
    }

    private IEnumerator GenIndicatorCoroutine(Vector3 _bossPos)
    {
        Vector3 indicatorPos = transform.position;
        indicatorPos.y += 35f;
        RaycastHit hit;

        while (!Physics.Raycast(indicatorPos, (_bossPos - indicatorPos).normalized, out hit, 10000f, 1 << LayerMask.NameToLayer("Boss")))
            yield return null;

        GameObject indicator = Instantiate(shieldGenIndicatorPrefab, indicatorPos, Quaternion.LookRotation(_bossPos - indicatorPos));
        indicator.transform.localScale = new Vector3(15f, 15f, Vector3.Distance(indicatorPos, hit.point) * 0.5f);
        indicator.transform.parent = transform;
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
    [SerializeField]
    private GameObject shieldGenIndicatorPrefab = null;
    [SerializeField]
    private LayerMask bossLayer;
}
