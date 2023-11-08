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

    private GroupHomingMissileSpawnPos[] arrGroupHomingMissileSpawnPos = null;
    private GameObject[] arrMissileGroup = new GameObject[32];
    private bool isArrayEmpty = false;
    protected override void OnStart() 
    {
        arrGroupHomingMissileSpawnPos = context.arrGroupHomingMissileSpawnPos;

        for (int i = 0; i < arrGroupHomingMissileSpawnPos.Length; ++i)
        {
            arrMissileGroup[i] = Instantiate(missilePrefab, arrGroupHomingMissileSpawnPos[i].GetPos(), arrGroupHomingMissileSpawnPos[i].GetRot());
            arrMissileGroup[i].GetComponent<GiantHomingMissileController>().Init(moveAccel, maxMoveSpeed, rotateAccel, maxRotateAccel, context.playerTr, autoDestroyTime);
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        for(int i = 0; i < arrMissileGroup.Length; ++i)
        {
            if (arrMissileGroup[i] != null)
            {
                return State.Running;
            }
        }

        Debug.Log("LaunchMissileGroup");
        return State.Success;
    }
}
