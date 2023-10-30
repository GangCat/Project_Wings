using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public void Init()
    {
        bossCtrl = GetComponentInChildren<BossController>();
        bossCtrl.Init(tree);
    }


    [SerializeField]
    private BehaviourTree tree = null;

    private BossController bossCtrl = null;
}
