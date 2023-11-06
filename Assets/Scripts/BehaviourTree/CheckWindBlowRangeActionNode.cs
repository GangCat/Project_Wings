using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEditor;

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
        for(int i = 0; i < weakPoints.Length; ++i)
        {
            if (Physics.OverlapSphere(weakPoints[i].GetPos(), range, 1 << LayerMask.NameToLayer("Player")).Length > 0)
            {
                blackboard.curClosedWeakPoint = i;
                return State.Success;
            }
        }

        return State.Failure;
    }

    public override void OnDrawGizmos()
    {
        for(int i = 0; i < weakPoints.Length; ++i)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(weakPoints[i].GetPos(), range);
        }
    }
}
