using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using System.Runtime.CompilerServices;
/// <summary>
/// 1. �÷��̾ ������ ���ö����� ������ �о�� ����
/// 2. ���� �ȿ� ���� ������ �÷��̾� ������ �ߵ�
/// 3. ������ �÷��̾��� ���� ����ؼ� �о
/// ����
/// �÷��̾��� ������ ������ ��� �ؾ��ϴ°� �÷��̾� ��ũ��Ʈ�� �ǵ鿩�� �ϳ�(bool ���?)
/// �� ���� �ſ��� ����ü �÷��̾�� ���� �о�� ���ΰ�
/// �̰Ͷ��� �ٶ� ��Ʈ�ѷ��� ����޾�� �ϴ°�
/// �Ʒ��� ���� ���� ������ context���� ���� ��Ʈ�ѷ����� �����ص� �Ǵ°�
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
