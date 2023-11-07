using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatusHp : StatusHp, IBossDamageable
{
    public void Init(VoidVoidDelegate _phaseChangeCallback)
    {
        curHp = maxHp;
        curPhaseNum = 1;
        phaseChangeCallback = _phaseChangeCallback;
    }

    public float GetCurHp => curHp;

    public void GetDamage(float _dmg)
    {
        curHp -= _dmg;

        if (curPhaseNum == 1 && curHp < maxHp * 0.5f)
            ChangePhase();
        else if (curPhaseNum == 2 && curHp < 0)
            ChangePhase();
    }

    private void ChangePhase()
    {
        ++curPhaseNum;
        phaseChangeCallback?.Invoke();
    }

    private VoidVoidDelegate phaseChangeCallback = null;
    private int curPhaseNum = 0;
}
