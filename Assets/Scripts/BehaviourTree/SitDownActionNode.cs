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
        //플레이어와의 거리 계산 > 가까울수록 소리 증가 > 보스 기계음 시작
        curDurationtime = durationTime;
    }

    protected override void OnStop()
    {
        context.anim.BossStandUp();
        context.sitDownGo.SetActive(false);
        // 보스 기계음 정지
    }

    protected override State OnUpdate()
    {
        curDurationtime -= Time.deltaTime;

        if (curDurationtime <= 0)
            return State.Success;

        return State.Running;
    }
}
