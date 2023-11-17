using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class NewLaunchMissileActionNode : ActionNode
{
    [SerializeField]
    private GameObject giantHomingMissilePrefab = null;
    [SerializeField]
    private float moveSpeed = 600f;
    [SerializeField]
    private float rotateSpeed_Degree = 3f;

    private Transform spawnTr = null;
    private GameObject crossLaserGo = null;

    protected override void OnStart() {
        context.anim.OpenBigMissileDoor();
        spawnTr = context.giantHomingMissileSpawnTr;
        crossLaserGo = Instantiate(giantHomingMissilePrefab, spawnTr.position, spawnTr.rotation);
        crossLaserGo.GetComponent<GiantHomingMissileController>().Init(context.playerTr.gameObject, moveSpeed, rotateSpeed_Degree, spawnTr.position, spawnTr.rotation, blackboard.isShieldDestroy, context.playerTr);
        context.bossCtrl.DangerAlert();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return crossLaserGo != null ? State.Running : State.Success;
    }
}
