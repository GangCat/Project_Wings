using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
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

    private float curDurationTime = 0f;
    protected override void OnStart() {
        context.airPushGo.SetActive(true);
        curDurationTime = durationTime;
    }

    protected override void OnStop() {
        context.airPushGo.SetActive(false);
    }

    protected override State OnUpdate() {
        curDurationTime -= Time.deltaTime;

        if(curDurationTime <= 0)
            return State.Success;
        return State.Running;
    }
}
