using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public void Init(Transform _PlayerTr)
    {
        bossCtrl = GetComponentInChildren<BossController>();
        bossCtrl.Init(_PlayerTr, gatlingHolderGo, gatlingHeadGo,gunMuzzleTr, giantHomingMissilePrefab, giantHomingMissileSpawnTr);
    }

    public void ClearCurPhase()
    {
        bossCtrl.ClearCurPhase();
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
