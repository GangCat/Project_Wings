using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SitDownActionNode : ActionNode
{
    [SerializeField]
    private float durationTime = 5f;
    protected override void OnStart()
    {
        context.sitDownGo.SetActive(true);
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        durationTime -= Time.deltaTime;
        if (durationTime <= 0)
        {
            context.sitDownGo.SetActive(false);
            return State.Success;
        }
        return State.Running;
    }
}
