using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class WindBlowAnimActionNode : ActionNode
{
    [SerializeField]
    private float animTime = 0f;
    [SerializeField]
    private bool isWindBlow = false;

    private float finishTime = 0f;
    protected override void OnStart() {
        //context.anim.SetBool("isWindBlowStart", isWindBlow);
        Debug.Log("start");
        finishTime = Time.time + animTime;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return Time.time < finishTime ? State.Running : State.Success;
    }
}
