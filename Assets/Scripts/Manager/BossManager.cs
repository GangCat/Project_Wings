using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public void Init(Transform _PlayerTr)
    {
        bossCtrl = GetComponentInChildren<BossController>();
        bossCtrl.Init(tree, _PlayerTr, gatlingHolderGo, gunMuzzleTr);
    }


    [SerializeField]
    private BehaviourTree tree = null;
    [SerializeField]
    private GameObject gatlingHolderGo = null;
    [SerializeField]
    private Transform gunMuzzleTr = null;

    private BossController bossCtrl = null;
}
