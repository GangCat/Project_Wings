using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class LaunchMissileGroupActionNode : ActionNode
{
    [SerializeField]
    private GameObject missilePrefab = null;
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
    [SerializeField]
    private float spawnRate = 0f;

    private GroupHomingMissileSpawnPos[] arrGroupHomingMissileSpawnPos = null;
    private GameObject[] arrMissileGroup = new GameObject[32];
    private bool isArrayEmpty = false;
    private int missileSpawnIdx = 0;
    private float startTime = 0;
    private bool isSpawnFinish = false;

    protected override void OnStart() 
    {
        arrGroupHomingMissileSpawnPos = context.arrGroupHomingMissileSpawnPos;
        startTime = Time.time;

        SpawnMissile();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (!isSpawnFinish && Time.time - startTime > spawnRate)
        {
            SpawnMissile();
            startTime = Time.time;
        }


        for (int i = 0; i < arrMissileGroup.Length; ++i)
        {
            if (arrMissileGroup[i].activeSelf)
            {
                return State.Running;
            }
        }


        Debug.Log("LaunchMissileGroup");
        return State.Success;
    }

    private void SpawnMissile()
    {
        for (int i = missileSpawnIdx; i < arrGroupHomingMissileSpawnPos.Length; i += 8)
        {
            //arrMissileGroup[i] = Instantiate(missilePrefab, arrGroupHomingMissileSpawnPos[i].GetPos(), arrGroupHomingMissileSpawnPos[i].GetRot());
            arrMissileGroup[i] = context.groupMissileMemoryPool.ActivateGroupMissile();
            Vector3 spawnPos = arrGroupHomingMissileSpawnPos[i].GetPos();
            Quaternion spawnRot = arrGroupHomingMissileSpawnPos[i].GetRot();
            arrMissileGroup[i].GetComponent<GroupHomingMissileController>().Init(moveAccel, maxMoveSpeed, rotateAccel, maxRotateAccel, context.playerTr, autoDestroyTime, spawnPos, spawnRot, context.groupMissileMemoryPool);
        }

        ++missileSpawnIdx;

        if (missileSpawnIdx >= 8)
            isSpawnFinish = true;
    }
}
