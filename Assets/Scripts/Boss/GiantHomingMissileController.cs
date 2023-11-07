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

            if (isPathBlock())
            {
                yield return waitFixed;
                continue;
            }

            RotateHomingMissile((targetTr.position - transform.position).normalized);

            yield return waitFixed;
        }
    }

    private bool isPathBlock()
    {
        return Physics.Linecast(transform.position, transform.forward * 500f, hitLayers);
    }

    private void MoveHomingMissile()
    {
        moveSpeed += moveAccel * Time.deltaTime;
        moveSpeed = Mathf.Min(moveSpeed, maxMoveSpeed);

        //float dotProduct = Vector3.Dot(transform.forward, (targetTr.position - transform.position).normalized);
        float dotProduct = Mathf.Clamp(Vector3.Dot(transform.forward, (targetTr.position - transform.position).normalized), -1f, 1f);

        // 내적 값을 기반으로 라디안 단위의 각도를 계산합니다.
        //float angleInRadians = Mathf.Acos(dotProduct);

        float normalizedAngle = Mathf.Acos(dotProduct) / Mathf.PI;

        float mappedValue = 1f - normalizedAngle;

        // 라디안을 도 단위로 변환합니다.
        //float angleInDegrees = Mathf.Rad2Deg * angleInRadians;

        //moveSpeed *= (mappedValue * 0.3f + 0.7f);

        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
    }

    private void RotateHomingMissile(Vector3 _moveDir)
    {
        //rotateSpeed += rotateAccel * Time.deltaTime;
        //rotateSpeed = Mathf.Min(rotateSpeed, maxRotateSpeed);
        rotateSpeed = maxRotateSpeed;

        //float dotProduct = Mathf.Clamp(Vector3.Dot(transform.forward, (targetTr.position - transform.position).normalized), -1f, 1f);
        //float normalizedAngle = Mathf.Acos(dotProduct) / Mathf.PI;
        //float mappedValue = 1f - normalizedAngle;

        //rotateSpeed *= mappedValue * 0.7f + 0.3f;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_moveDir), rotateSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Obstacle"))
            Destroy(gameObject);
        else if (_other.CompareTag("Floor"))
            Destroy(gameObject);
        else if (AttackDmg(_other))
        {
            if (isFirstTrigger)
            {
                isFirstTrigger = false;
                return;
            }

            Destroy(gameObject);
        }
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
