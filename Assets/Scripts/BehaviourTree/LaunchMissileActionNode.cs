using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System.Reflection;
using System.Runtime.CompilerServices;
/// <summary> 
/// ȣ�� �� ������ �̻��� ���� 
/// �̻��� ������, �÷��̾� ��ġ, ���� ��ġ���� ���� �޾ƾ� �� 
/// �̻����� ���� ������ runninng ��ȯ 
/// �̻����� ���� �ӵ��� �̵� 
/// �ð��� �������� ���ӵ��� ���� 
/// �÷��̾� �������� ȸ�� 
/// ȸ�� ���� �� ���� �ӵ� ���� ���ӵ��� ���� �پ��
/// ȸ�� ���� �ƴϰ� �ִ� �ӵ����� ���ӵ��� ���� �þ
/// ������ �Ӹ� �������� ���� ���⺤�� ���� �� ���ư� 
/// �ʿ��� ���� : �̻��� ������, �÷��̾� ��ġ, ������ ���� ��ġ, ���� �ӵ�, �ְ� �ӵ�, ���ӵ�, ���� ���� ���� 
/// </summary>
public class LaunchMissileActionNode : ActionNode
{
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float autoDestroyTime;

    private float currentSpeed;
    private bool isRotating = false;
    private Vector3 directionVector;
    private Quaternion lastRotation;
    GameObject missile;
    protected override void OnStart()
    {
        missile = Instantiate(context.giantHomingMissileGo, context.giantHomingMissileSpawnTr.position, Quaternion.identity);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (missile)
        {
            AutoDestroy();
            // Rotate towards the player
            Vector3 direction = context.playerTr.position - missile.transform.position;
            missile.transform.rotation = Quaternion.Slerp(missile.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);

            // Increase acceleration and speed over time
            currentSpeed = minSpeed;
            if (!IsRotating())
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += acceleration * Time.deltaTime;
                }
            }
            else
            {
                if (currentSpeed > minSpeed)
                {
                    currentSpeed -= acceleration * Time.deltaTime;
                }
            }
            // Move missile in the direction of rotation
            directionVector = missile.transform.forward;
            Debug.Log("Homing Missile Current Speed:");
            Debug.Log(currentSpeed);
            missile.transform.position += directionVector * currentSpeed * Time.deltaTime;
            return State.Running;
        }
        else
        {
            return State.Success;
        }
        
        
        
    }
    private bool IsRotating()
    {
        if (lastRotation != missile.transform.rotation)
        {
            lastRotation = missile.transform.rotation;
            return true;
        }
        else
        {
            return false;
        }
    }
    private void AutoDestroy()
    {
        autoDestroyTime -= Time.deltaTime;
        if (autoDestroyTime <= 0f) Destroy(missile);
    }
}
