using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public void Init(Transform _PlayerTr, VoidIntDelegate _cameraActionCallback)
    {
        bossCtrl = GetComponentInChildren<BossController>();
        bossCtrl.Init(_PlayerTr, _cameraActionCallback);
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
