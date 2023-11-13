using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CrossLazerActionNode : ActionNode
{
    [SerializeField]
    private GameObject crossLaserPrefab = null;
    [SerializeField]
    private float moveAccel = 300f;
    [SerializeField]
    private float maxMoveSpeed = 500f;
    [SerializeField]
    private float rotateAccel = 5f;
    [SerializeField]
    private float maxRotateAccel = 5f;
    [SerializeField]
    private float autoDestroyTime = 20f;
    [SerializeField]
    private float changeFormDistnacec = 500f;
    [SerializeField]
    private float patternFinishTime = 10f;

    private float patternStartTime = 0f;
    private Transform spawnTr = null;
    private GameObject crossLaserGo = null;

    protected override void OnStart() {
        spawnTr = context.giantHomingMissileSpawnTr;
        crossLaserGo = Instantiate(crossLaserPrefab, spawnTr.position, spawnTr.rotation);
        crossLaserGo.GetComponent<CrossLaserController>().Init(moveAccel, maxMoveSpeed, rotateAccel, maxRotateAccel, changeFormDistnacec, context.playerTr, autoDestroyTime);

        patternStartTime = Time.time;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return Time.time - patternStartTime < patternFinishTime ? State.Running : State.Success;
    }
}
