using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPatternRange : ActionNode
{
    [SerializeField]
    private float range = 0f;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(Physics.OverlapBox(context.transform.position, Vector3.one * range, context.transform.rotation, 1<<LayerMask.NameToLayer("Player")).Length > 0)
            return State.Success;
        else
            return State.Failure;
    }
}
