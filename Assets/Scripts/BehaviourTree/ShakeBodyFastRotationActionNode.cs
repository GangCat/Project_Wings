using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ShakeBodyFastRotationActionNode : ActionNode
{
    [SerializeField]
    private float rotationSpeed = 0f;
    [SerializeField]
    private float rotationLimitAngle = 0f;
    [SerializeField]
    private float rotationAccel = 0f;
    [SerializeField]
    private float attackRange = 0f;
    [SerializeField]
    private LayerMask playerLayer;
    

    private float curRotationSpeed = 0f;
    private Transform bossTr = null;
    private BoxCollider shakeBodyCollider = null;

    protected override void OnStart() {
        bossTr = context.transform;
        shakeBodyCollider = context.boxCollider;
        shakeBodyCollider.enabled = true;
        shakeBodyCollider.center = Vector3.zero;
        shakeBodyCollider.size = Vector3.one * attackRange;
    }

    protected override void OnStop() {
        shakeBodyCollider.enabled = false;
    }

    protected override State OnUpdate() {
        curRotationSpeed = Mathf.MoveTowards(curRotationSpeed, rotationSpeed, rotationAccel * Time.deltaTime);
        bossTr.rotation = Quaternion.Euler(Vector3.down * rotationSpeed * Time.deltaTime);

        if (bossTr.rotation.y > rotationLimitAngle)
            return State.Running;
        else
            return State.Success;
    }

    private void windAttack()
    {

    }
}
