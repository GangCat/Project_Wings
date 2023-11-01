using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhaseOneRange : ActionNode
{
    [SerializeField]
    private float range = 0f;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (Vector3.SqrMagnitude(context.transform.position - context.playerTr.position) < Mathf.Pow(range, 2f))
            return State.Success;
        else
            return State.Failure;
    }
}
