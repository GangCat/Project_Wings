using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public void Init(Transform _PlayerTr, VoidIntDelegate _cameraActionCallback, VoidFloatDelegate _hpUpdateCallback)
    {
        bossCtrl = GetComponentInChildren<BossController>();
        bossCtrl.Init(_PlayerTr, _cameraActionCallback, _hpUpdateCallback);
    }

    public void ClearCurPhase()
    {
        bossCtrl.ClearCurPhase();
    }

    public void ActionFinish()
    {
        bossCtrl.FinishPhaseChange();
    }

    public void GameStart()
    {
        bossCtrl.GameStart();
    }

    private BossController bossCtrl = null;
}
