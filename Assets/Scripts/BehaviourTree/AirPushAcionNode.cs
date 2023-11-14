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
    private Vector3 attackStartRange;
    [SerializeField]
    private Vector3 attackRange;
    [SerializeField]
    private float pushForce = 10f;
    [SerializeField]
    private float accel = 0.1f;

    private Transform bossTr;
    private Transform player;
    

    protected override void OnStart() {
        bossTr.position = new Vector3(0, context.playerTr.position.y, 0);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(bossTr.position, player.position);

        // �÷��̾ ���� ���� �ȿ� ������ �о�� ���� �ߵ�
        if (CheckPlayer())
        {
            attackStartRange = attackRange;
            PushPlayer();
        }
        return State.Success;
    }
    private bool CheckPlayer()
    {
        Collider[] playerCol = Physics.OverlapBox(bossTr.position, attackStartRange / 2f, Quaternion.identity, 6);
        if (playerCol != null) return true;
        else return false;
    }
    private void PushPlayer()
    {
        // ������ �÷��̾��� ���� ���
        Vector3 directionToPlayer = (player.position - bossTr.position).normalized;
        pushForce += accel;
        // �÷��̾ �о�� �� ����
        player.GetComponent<Rigidbody>().AddForce(directionToPlayer * pushForce, ForceMode.Impulse);
    }
}
