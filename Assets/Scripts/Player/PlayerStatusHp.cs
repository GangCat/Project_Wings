using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� ü���� �ǰ�Ƚ�� ����
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
                Debug.Log("���������̻��� ��Ʈ");
        }
        else if (_dmg == 100)
        {
            if (GetComponent<PlayerController>())
                Debug.Log("���������̻��� ��Ʈ");
        }
    }

    public void Heal()
    {
        curHp = maxHp;
    }

    private VoidVoidDelegate deadCallback = null;
}
