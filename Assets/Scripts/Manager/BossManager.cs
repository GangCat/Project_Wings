using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public void Init(Transform _PlayerTr)
    {
        bossCtrl = GetComponentInChildren<BossController>();
        //bossCtrl.Init(tree, _PlayerTr);
    }


    [SerializeField]
    private BehaviourTree tree = null;

    private BossController bossCtrl = null;
}
