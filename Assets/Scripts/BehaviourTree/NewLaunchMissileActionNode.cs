using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class NewLaunchMissileActionNode : ActionNode
{
    [SerializeField]
    private GameObject giantHomingMissilePrefab = null;
    [SerializeField]
    private float moveAccel = 0f;
    [SerializeField]
    private float maxMoveSpeed = 0f;
    [SerializeField]
    private float rotateAccel = 0f;
    [SerializeField]
    private float maxRotateAccel = 0f;
    [SerializeField]
    private float autoDestroyTime = 0f;

    private Transform spawnTr = null;
    private GameObject crossLaserGo = null;

    protected override void OnStart() {
        spawnTr = context.giantHomingMissileSpawnTr;
        crossLaserGo = Instantiate(giantHomingMissilePrefab, spawnTr.position, spawnTr.rotation);
        crossLaserGo.GetComponent<GiantHomingMissileController>().Init(moveAccel, maxMoveSpeed, rotateAccel, maxRotateAccel, context.playerTr, autoDestroyTime);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return crossLaserGo != null ? State.Running : State.Success;
    }
}
