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

        windBlowPoints = context.secondWeakPointHolder.WeakPoints[blackboard.curClosedWeakPoint].GetWindBlowHolder().WindBlowPoints;
        foreach (WindBlowPoint wbp in windBlowPoints)
            wbp.StartGenerateSecond(windCylinderPrefab);

        finishTime = Time.time + totalDuration;
    }

    protected override void OnStop() {
        if (windBlowPoints != null)
        {
            foreach (WindBlowPoint wbp in windBlowPoints)
                wbp.FinishGenerate();
        }
    }

    protected override State OnUpdate() {
        if (Time.time >= finishTime)
            return State.Success;

        return State.Running;
    }
}
