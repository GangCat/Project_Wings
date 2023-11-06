using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ShakeBodySlowRotationActionNode : ActionNode
{
    [SerializeField]
    private float maxRotSpeed = 20f;
    [SerializeField]
    private float rotationLimitAngle = 30f;
    [SerializeField]
    private float rotationAccel = 20f;

    private float curRotation = 0f;
    private float curRotationSpeed = 0f;
    private float curRotationAngle = 0f;
    private Transform bossTr = null;

    protected override void OnStart() {
        bossTr = context.transform;
        curRotationAngle = bossTr.rotation.eulerAngles.y;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        curRotationSpeed += curRotationSpeed < maxRotSpeed ? rotationAccel * Time.deltaTime : 0;
        curRotationAngle += curRotationSpeed * Time.deltaTime;
        bossTr.rotation = Quaternion.Euler(Vector3.up * curRotationAngle);

        curRotation = bossTr.rotation.eulerAngles.y;
        if (curRotation > 180)
            curRotation -= 360;

        if (curRotation < rotationLimitAngle)
        {
            return State.Running;
        }
        else
        {
            return State.Success;
        }
    }
}
