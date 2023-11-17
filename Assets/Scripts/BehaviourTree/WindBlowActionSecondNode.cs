using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class WindBlowActionSecondNode : ActionNode
{
    [SerializeField]
    private float totalDuration = 5f;
    [SerializeField]
    private GameObject windCylinderPrefab;

    private WindBlowPoint[] windBlowPoints = null;
    private float finishTime = 0f;

    protected override void OnStart() {

        windBlowPoints = context.bossCtrl.CurSpawnPoints[blackboard.curClosedWeakPoint].GetWindBlowHolder().WindBlowPoints;
        foreach (WindBlowPoint wbp in windBlowPoints)
            wbp.StartGenerateSecond(windCylinderPrefab);
        //�ٶ� �Ҹ� ����(����)
        finishTime = Time.time + totalDuration;
    }

    protected override void OnStop() {
        if (windBlowPoints != null)
        {
            //���� �� �˻� �ѹ��ϰ� �ξƴϸ� ���� ����
            foreach (WindBlowPoint wbp in windBlowPoints)
                wbp.FinishGenerate();
        }
    }

    protected override State OnUpdate() {
        if (blackboard.isPhaseEnd || Time.time >= finishTime)
        {
            return State.Success;
        }
        return State.Running;
    }
}
