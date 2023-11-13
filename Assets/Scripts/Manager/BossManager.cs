using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public void Init(Transform _PlayerTr, VoidIntDelegate _cameraActionCallback, VoidFloatDelegate _hpUpdateCallback, BossController.GetRandomSpawnPointDelegate _callback, VoidVoidDelegate _bossClearCallback)
    {
        bossCtrl = GetComponentInChildren<BossController>();
        bossCtrl.Init(_PlayerTr, _cameraActionCallback, _hpUpdateCallback, _callback, _bossClearCallback);
    }

    public void ClearCurPhase()
    {
        bossCtrl.ClearShieldGenerator();
    }

    public void ActionFinish()
    {
        bossCtrl.PatternStart();
    }

    public void JumpToNextPattern()
    {
        bossCtrl.JumpToNextPhase();
    }

    public void GameStart()
    {
        bossCtrl.GameStart();
    }

    private BossController bossCtrl = null;
}
