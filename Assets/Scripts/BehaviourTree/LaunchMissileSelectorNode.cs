using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class LaunchMissileSelectorNode : CompositeNode
{
    [SerializeField, Range(0, 1)]
    private float groupMissileRatio = 0f;
    [SerializeField, Range(0, 1)]
    private float curGroupMissileRatio = 0f;

    private int rndNum = 0;
    protected override void OnStart() {
        // 난수 생성
        rndNum = Random.Range(0, 100);
    }

    protected override void OnStop() {
        // 작은 미사일 확률이 높지만 계속 작은 미사일만 나오지 않도록
        // 작은 미사일이 선택되었을 경우 해당 확률을 적게 조정
        if (rndNum < curGroupMissileRatio * 100)
            curGroupMissileRatio -= 0.1f;
        else
            curGroupMissileRatio = groupMissileRatio;
    }

    protected override State OnUpdate() {
        // 난수를 이용해 확률적으로 작은 미사일/큰 미사일 선택
        if (rndNum < curGroupMissileRatio * 100)
            return children[0].Update();
        else
            return children[1].Update();
    }
}
