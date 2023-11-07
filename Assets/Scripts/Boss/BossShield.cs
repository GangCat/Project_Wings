using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour, IDamageable
{
    public void Init()
    {

    }

    public void RespawnGenerator()
    {
        curGeneratorCount = 4;
    }

    public void SetActive(bool _active)
    {
        gameObject.SetActive(_active);
    }

    public void GeneratorDestroy()
    {
        --curGeneratorCount;
        UpdateEffect();
    }

    private void UpdateEffect()
    {
        // ���̴����� ��ġ 1, 0.75, 0.5, 0.25 ����
        // curGeneratorCount * 0.25f;
        // �� �� ������.
    }

    public float GetCurHp => 99999;

    public void GetDamage(float _dmg)
    {
        Debug.Log($"Shield Hit!");
    }

    private int curGeneratorCount = 0;
}
