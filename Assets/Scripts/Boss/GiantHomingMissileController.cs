using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class GiantHomingMissileController : AttackableObject
{
    public void Init(float _moveAccel, float _maxMoveSpeed, float _rotateAccel, float _maxRotateAccel, Transform _targetTr, float _autoDestroyTime)
    {
        moveAccel = _moveAccel;
        maxMoveSpeed = _maxMoveSpeed;
        rotateAccel = _rotateAccel;
        maxRotateSpeed = _maxRotateAccel;
        targetTr = _targetTr;
        waitFixed = new WaitForFixedUpdate();
        moveSpeed = 0f;
        rotateSpeed = 0f;

        Destroy(gameObject, _autoDestroyTime);

        StartCoroutine(MoveUpdateCoroutine());
    }

    private IEnumerator MoveUpdateCoroutine()
    {
        while (true)
        {
            MoveHomingMissile();
            Debug.Log($"MissileSpeed: {moveSpeed}");

            if (IsPathBlock()/* || IsTargetNear()*/)
            {
                yield return waitFixed;
                continue;
            }

            RotateHomingMissile((targetTr.position - transform.position).normalized);

            yield return waitFixed;
        }
    }

    private bool IsPathBlock()
    {
        return Physics.Linecast(transform.position, transform.forward * 500f, hitLayers);
    }

    private bool IsTargetNear()
    {
        return Vector3.SqrMagnitude(transform.position - targetTr.position) < Mathf.Pow(500, 2f);
    }

    private void MoveHomingMissile()
    {
        moveSpeed += moveAccel * Time.deltaTime;
        moveSpeed = Mathf.Min(moveSpeed, maxMoveSpeed);

        //float dotProduct = Vector3.Dot(transform.forward, (targetTr.position - transform.position).normalized);
        float dotProduct = Mathf.Clamp(Vector3.Dot(transform.forward, (targetTr.position - transform.position).normalized), -1f, 1f);

        // ���� ���� ������� ���� ������ ������ ����մϴ�.
        //float angleInRadians = Mathf.Acos(dotProduct);

        float normalizedAngle = Mathf.Acos(dotProduct) / Mathf.PI;

        float mappedValue = 1f - normalizedAngle;

        // ������ �� ������ ��ȯ�մϴ�.
        //float angleInDegrees = Mathf.Rad2Deg * angleInRadians;

        //moveSpeed *= (mappedValue * 0.5f + 0.5f);

        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
    }

    private void RotateHomingMissile(Vector3 _moveDir)
    {
        //rotateSpeed += rotateAccel * Time.deltaTime;
        //rotateSpeed = Mathf.Min(rotateSpeed, maxRotateSpeed);
        //rotateSpeed = maxRotateSpeed;

        //float dotProduct = Mathf.Clamp(Vector3.Dot(transform.forward, (targetTr.position - transform.position).normalized), -1f, 1f);
        //float normalizedAngle = Mathf.Acos(dotProduct) / Mathf.PI;

        Quaternion targetRotation = Quaternion.LookRotation(targetTr.position);
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

        // ��ǥ ȸ���� �������� ���� ��� ���ӵ��� ���Ͽ� ȸ�� �ӵ��� ����
        if (angleDifference > 0.1f)
        {
            rotateSpeed += rotateAccel * Time.deltaTime;
        }
        else
        {
            // ��ǥ ȸ���� ������ ��� ���ӵ� �ʱ�ȭ
            rotateSpeed = 0.0f;
        }
        //float mappedValue = 1f - normalizedAngle;

        //if(normalizedAngle > 0.95f)

        //rotateSpeed *= normalizedAngle;

        //transform.rotation *= Quaternion.LookRotation(_moveDir) * rotateSpeed * Time.fixedDeltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_moveDir), rotateSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Obstacle"))
            Destroy(gameObject);
        else if (_other.CompareTag("Floor"))
            Destroy(gameObject);
        else if (isFirstTrigger)
            return;

        if (AttackDmg(_other))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BossShield"))
            isFirstTrigger = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 250f);
    }


    private WaitForFixedUpdate waitFixed = null;

    private float moveAccel = 0f;
    private float moveSpeed = 0f;
    private float maxMoveSpeed = 0f;
    private float rotateAccel = 0f;
    private float rotateSpeed = 0f;
    private float maxRotateSpeed = 0f;

    private Transform targetTr = null;

    [SerializeField]
    private LayerMask hitLayers;
}
