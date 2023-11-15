using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using System.Runtime.CompilerServices;
/// <summary>
/// 1. 플레이어가 안으로 들어올때마다 보스가 밀어내는 패턴
/// 2. 보스 안에 일정 범위에 플레이어 있으면 발동
/// 3. 보스와 플레이어의 방향 계산해서 밀어냄
/// 문제
/// 플레이어의 움직임 강제를 어떻게 해야하는가 플레이어 스크립트를 건들여야 하나(bool 사용?)
/// 몸 흔드는 거에서 도대체 플레이어는 어케 밀어내는 것인가
/// 이것또한 바람 컨트롤러를 사용햇어야 하는가
/// 아래의 공격 범위 지정은 context에서 보스 컨트롤러에서 지정해도 되는가
/// </summary>
public class AirPushAcionNode : ActionNode
{

    [SerializeField]
    private float durationTime = 5f;
    protected override void OnStart() {
        context.airPushGo.SetActive(true);
    }

    protected override void OnStop() {
        
    }

    protected override State OnUpdate() {
        durationTime -= Time.deltaTime;
        if(durationTime <= 0)
        {
            context.airPushGo.SetActive(false);
            return State.Success;
        }
        return State.Running;
    }
}
