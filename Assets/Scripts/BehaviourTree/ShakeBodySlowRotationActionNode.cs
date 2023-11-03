using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ShakeBodySlowRotationActionNode : ActionNode
{
    [SerializeField]
    private float rotationSpeed = 0f;
    [SerializeField]
    private float rotationLimitAngle = 0f;
    [SerializeField]
    private float rotationAccel = 0f;

    private float curRotationSpeed = 0f;
    private Transform bossTr = null;

    protected override void OnStart() {
        bossTr = context.transform;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        curRotationSpeed = Mathf.MoveTowards(curRotationSpeed, rotationSpeed, rotationAccel * Time.deltaTime);
        bossTr.rotation = Quaternion.Euler(Vector3.up * rotationSpeed * Time.deltaTime);

        if (bossTr.rotation.y < rotationLimitAngle)
            return State.Running;
        else
            return State.Success;
    }
}
