using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 체력은 피격횟수 기준
/// </summary>
public class PlayerStatusHp : StatusHp, IPlayerDamageable
{
    public void Init(VoidVoidDelegate _deadCallback, VoidFloatDelegate _hpUpdateCallback)
    {
        curHp = maxHp;
        deadCallback = _deadCallback;
        hpUpdateCallback = _hpUpdateCallback;
    }

    public float GetCurHp => curHp;

    public void GetDamage(float _dmg)
    {
        if (gameObject.layer.Equals(LayerMask.NameToLayer("PlayerInvincible")))
            return;

        curHp -= _dmg;

        hpUpdateCallback?.Invoke(curHp / maxHp);

        if (curHp <= 0)
            deadCallback?.Invoke();

    }

    public void MaxHeal()
    {
        curHp = maxHp;
    }

    private VoidVoidDelegate deadCallback = null;
    private VoidFloatDelegate hpUpdateCallback = null;
}
