using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System.Reflection;
using System.Runtime.CompilerServices;
/// <summary> 
/// 호출 될 때마다 미사일 생성 
/// 미사일 프리팹, 플레이어 위치, 생성 위치등을 전달 받아야 함 
/// 미사일이 아직 있으면 runninng 반환 
/// 미사일은 최저 속도로 이동 
/// 시간이 지날수록 가속도가 붙음 
/// 플레이어 방향으로 회전 
/// 회전 중일 시 최저 속도 까지 가속도가 점점 줄어듬
/// 회전 중이 아니고 최대 속도까지 가속도가 점점 늘어남
/// 본인의 머리 방향으로 직선 방향벡터 생성 후 날아감 
/// 필요한 변수 : 미사일 프리팹, 플레이어 위치, 본인의 생성 위치, 최저 속도, 최고 속도, 가속도, 직진 방향 벡터 
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
