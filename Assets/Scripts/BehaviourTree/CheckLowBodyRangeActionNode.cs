using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckLowBodyRangeActionNode : ActionNode
{
    [SerializeField]
    private float range = 1000f;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (blackboard.curPhaseNum != 2)
            return State.Failure;

        if (Physics.OverlapSphere(context.transform.position + Vector3.down * (range * 0.5f), range, 1 << LayerMask.NameToLayer("Player")).Length > 0)
            return State.Success;


        return State.Success;
    }
}
