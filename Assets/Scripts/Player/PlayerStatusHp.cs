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
        if (gameObject.layer.Equals(LayerMask.NameToLayer("PlayerInvincible")))
            return;

        curHp -= _dmg;
        if (curHp <= 0)
            deadCallback?.Invoke();

        if (_dmg == 20)
        {
            if(GetComponent<PlayerController>())
                Debug.Log("소형유도미사일 히트");
        }
        else if (_dmg == 100)
        {
            if (GetComponent<PlayerController>())
                Debug.Log("대형유도미사일 히트");
        }
    }

    public void Heal()
    {
        curHp = maxHp;
    }

    private VoidVoidDelegate deadCallback = null;
}
