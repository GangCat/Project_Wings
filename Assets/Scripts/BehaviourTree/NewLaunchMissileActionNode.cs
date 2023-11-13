using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class NewLaunchMissileActionNode : ActionNode
{
    [SerializeField]
    private GameObject giantHomingMissilePrefab = null;
    [SerializeField]
    private float moveAccel = 400f;
    [SerializeField]
    private float maxMoveSpeed = 600f;
    [SerializeField]
    private float rotateAccel = 0.2f;
    [SerializeField]
    private float maxRotateAccel = 3f;
    [SerializeField]
    private float autoDestroyTime = 20f;

    private Transform spawnTr = null;
    private GameObject crossLaserGo = null;

    protected override void OnStart() {
        spawnTr = context.giantHomingMissileSpawnTr;
        crossLaserGo = Instantiate(giantHomingMissilePrefab, spawnTr.position, spawnTr.rotation);
        crossLaserGo.GetComponent<GiantHomingMissileController>().Init(moveAccel, maxMoveSpeed, rotateAccel, maxRotateAccel, context.playerTr, autoDestroyTime, blackboard.isShieldDestroy);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return crossLaserGo != null ? State.Running : State.Success;
    }
}
