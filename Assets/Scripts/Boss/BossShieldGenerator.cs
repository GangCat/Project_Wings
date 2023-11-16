using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldGenerator : MonoBehaviour, IDamageable
{
    public void Init(VoidGameObjectDelegate _destroyCallback, Vector3 _bossPos)
    {
        destroyCallback = _destroyCallback;
        curHp = maxHp;

        StartCoroutine(GenIndicatorCoroutine(_bossPos));
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator GenIndicatorCoroutine(Vector3 _bossPos)
    {
        //�÷��̾���� �Ÿ���� > �������� �Ҹ� ���� > ���� �����Ҹ� �߻� 
        Vector3 indicatorPos = transform.position;
        indicatorPos.y += 35f;
        RaycastHit hit;

        while (!Physics.Raycast(indicatorPos, (_bossPos - indicatorPos).normalized, out hit, 10000f, 1 << LayerMask.NameToLayer("Boss")))
            yield return null;

        GameObject indicator = Instantiate(shieldGenIndicatorPrefab, indicatorPos, Quaternion.LookRotation(_bossPos - indicatorPos));
        indicator.transform.localScale = new Vector3(15f, 15f, Vector3.Distance(indicatorPos, hit.point) * 0.5f);
        indicator.transform.parent = transform;
    }

    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            for(int i = 0; i < rotateModelTr.Length; ++i)
                rotateModelTr[i].rotation *= Quaternion.Euler(Vector3.one * (i + 1) * 8f * Time.deltaTime);

            yield return new WaitForFixedUpdate();
        }
    }

    public float GetCurHp => curHp;

    public void GetDamage(float _dmg)
    {
        if (curHp < 0)
            return;
        //�÷��̾���� �Ÿ� ��� > �������� �Ҹ� ũ�� > �� ���� �Ҹ� �߻�
        curHp -= _dmg;

        if (curHp < 0)
        {
            destroyCallback?.Invoke(gameObject);
            //���� ����� �ı��Ǵ¼Ҹ� ���
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

    [Header("InformationForRotation")]
    [SerializeField]
    private Transform[] rotateModelTr = null;
}
