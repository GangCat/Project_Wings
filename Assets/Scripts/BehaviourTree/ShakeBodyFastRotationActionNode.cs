using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ShakeBodyFastRotationActionNode : ActionNode
{
    [SerializeField]
    private float maxRotSpeed = 0f;
    [SerializeField]
    private float rotationLimitAngle = 0f;
    [SerializeField]
    private float rotationAccel = 0f;
    [SerializeField]
    private float attackRange = 0f;
    [SerializeField]
    private string shakeBodyTag = "";
    [SerializeField]
    private GameObject effectTornado = null;

    private float curRotation = 0f;
    private float curRotationAngle = 0f;
    private float curRotationSpeed = 0f;
    private Transform bossTr = null;
    private BossCollider bossCollider = null;
    private GameObject tornadoGo = null;
    private Rigidbody bossRb = null;

    protected override void OnStart() {
        bossTr = context.transform;
        curRotationAngle = bossTr.rotation.eulerAngles.y;
        bossRb = context.physics;
        windAttackInit();
    }

    protected override void OnStop() {
        if (bossCollider)
        {
            bossCollider.ResetAll();
            Destroy(tornadoGo);
        }
    }

    protected override State OnUpdate() {
        curRotationSpeed += curRotationSpeed < maxRotSpeed ? rotationAccel * Time.deltaTime : 0;
        bossRb.MoveRotation(bossRb.rotation * Quaternion.Euler(Vector3.down * curRotationSpeed * Mathf.Deg2Rad));
        //curRotationSpeed += rotationAccel * Time.deltaTime;
        //curRotationSpeed = Mathf.Min(curRotationSpeed, maxRotSpeed);
        //curRotationAngle -= curRotationSpeed * Time.deltaTime;
        //bossTr.rotation = Quaternion.Euler(Vector3.up * curRotationAngle);

        curRotation = bossTr.rotation.eulerAngles.y;
        if (curRotation > 180)
            curRotation -= 360;

        return curRotation > rotationLimitAngle ? State.Running : State.Success;
        //if (curRotation > rotationLimitAngle)
        //{
        //    //Debug.Log(bossTr.rotation.eulerAngles.y);
        //    return State.Running;
        //}
        //else
        //    return State.Success;
    }

    private void windAttackInit()
    {
        bossCollider = context.bossCollider;
        bossCollider.SetEnableCollider();
        bossCollider.SetPos(bossTr.position);
        bossCollider.SetSize(attackRange);
        bossCollider.SetTag(shakeBodyTag);

        tornadoGo = Instantiate(effectTornado, bossTr.position, Quaternion.identity);

    }
}
