using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 체력은 피격횟수 기준
/// </summary>
public class PlayerStatusHp : StatusHp, IPlayerDamageable
{
    public void Init(VoidVoidDelegate _deadCallback)
    {
        curHp = maxHp;
        deadCallback = _deadCallback;
    }

    public float GetCurHp => curHp;

    public void GetDamage(float _dmg)
    {
        curHp -= _dmg;
        if (curHp <= 0)
            deadCallback?.Invoke();
    }

    public void Heal()
    {
        curHp = maxHp;
    }

    private VoidVoidDelegate deadCallback = null;
}
