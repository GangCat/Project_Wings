using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SitDownActionNode : ActionNode
{
    [SerializeField]
    private float durationTime = 5f;

    private float curDurationtime = 0f;
    protected override void OnStart()
    {
        context.anim.bossSitDown();
        context.sitDownGo.SetActive(true);
        //�÷��̾���� �Ÿ� ��� > �������� �Ҹ� ���� > ���� ����� ����
        curDurationtime = durationTime;
    }

    protected override void OnStop()
    {
        context.anim.BossStandUp();
        context.sitDownGo.SetActive(false);
        // ���� ����� ����
    }

    protected override State OnUpdate()
    {
        curDurationtime -= Time.deltaTime;

        if (curDurationtime <= 0)
            return State.Success;

        return State.Running;
    }
}
