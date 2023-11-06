using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public void Init(Transform _PlayerTr, VoidIntDelegate _cameraActionCallback)
    {
        bossCtrl = GetComponentInChildren<BossController>();
        bossCtrl.Init(_PlayerTr, gatlingHolderGo, gatlingHeadGo,gunMuzzleTr, giantHomingMissilePrefab, giantHomingMissileSpawnTr, _cameraActionCallback);
    }

    public void ClearCurPhase()
    {
        bossCtrl.ClearCurPhase();
    }

    public void ActionFinish()
    {
        bossCtrl.FinishPhaseChange();
    }


    [SerializeField]
    private GameObject gatlingHolderGo = null;
    [SerializeField]
    private GameObject gatlingHeadGo = null;
    [SerializeField]
    private Transform gunMuzzleTr = null;
    [SerializeField]
    private Transform giantHomingMissileSpawnTr = null;
    [SerializeField]
    private GameObject giantHomingMissilePrefab = null;

    private BossController bossCtrl = null;
}
