using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckWindBlowRangeActionNode : ActionNode
{
    [SerializeField]
    private float range = 0f;
    private WeakPoint[] weakPoints = null;

    protected override void OnStart() {
        weakPoints = context.secondWeakPointHolder.WeakPoints;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        foreach(WeakPoint wp in weakPoints)
        {
            if (Physics.OverlapSphere(wp.GetPos(), range, 1 << LayerMask.NameToLayer("Player")).Length > 0)
                return State.Success;
        }

        return State.Failure;
    }
}
