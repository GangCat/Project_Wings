using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        moveSpeed = maxMoveSpeed;
        rotateSpeed = 0f;

        Destroy(gameObject, _autoDestroyTime);


        StartCoroutine(MoveUpdateCoroutine());
    }

    private IEnumerator MoveUpdateCoroutine()
    {
        while (true)
        {
            MoveHomingMissile();
            RotateHomingMissile((targetTr.position - transform.position).normalized);
            //Debug.Log($"MissileSpeed: {moveSpeed}");

            yield return waitFixed;
        }
    }

    private void MoveHomingMissile()
    {
        moveSpeed += moveAccel * Time.deltaTime;
        moveSpeed = Mathf.Min(moveSpeed, maxMoveSpeed);

        dotProduct = Mathf.Clamp(Vector3.Dot(transform.forward, (targetTr.position - transform.position).normalized), -1f, 1f);
        normalizedAngle = Mathf.Acos(dotProduct) / Mathf.PI;
        mappedValue = 1f - normalizedAngle;

        moveSpeed *= (mappedValue * 0.3f + 0.7f);
        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
    }

    private void RotateHomingMissile(Vector3 _moveDir)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetTr.position);
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

        if (angleDifference > 0.1f)
        {
            rotateSpeed += rotateAccel * Time.deltaTime;
        }
        else
        {
            rotateSpeed = 0.0f;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_moveDir), rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (isFirstTrigger)
            return;

        Explosion();
        //if (_other.CompareTag("Obstacle"))
        //    Explosion();
        //else if (_other.CompareTag("Floor"))
        //    Explosion();
        //else if (isFirstTrigger)
        //    return;
        //else if (_other.CompareTag("GiantHomingMissile"))
        //    Explosion();

        //if (AttackDmg(_other))
        //{
        //    Explosion();
        //}
    }

    public void Explosion()
    {
        if (isExplosed)
            return;

        isExplosed = true;
        Collider[] arrTempCollider = Physics.OverlapSphere(transform.position, explosionRange, explosionLayer);
        foreach(Collider col in arrTempCollider)
        {
            if (col.CompareTag("GiantHomingMissile"))
            {
                col.GetComponent<GiantHomingMissileController>().Explosion();
            }
            else
                AttackDmg(col);
        }

        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BossShield"))
            isFirstTrigger = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }


    private WaitForFixedUpdate waitFixed = null;

    private float moveAccel = 0f;
    private float moveSpeed = 0f;
    private float maxMoveSpeed = 0f;
    private float rotateAccel = 0f;
    private float rotateSpeed = 0f;
    private float maxRotateSpeed = 0f;

    float dotProduct = 0f;
    float normalizedAngle = 0f;
    float mappedValue = 0f;

    private Transform targetTr = null;
    private bool isExplosed = false;

    [SerializeField]
    private GameObject explosionEffectPrefab;
    [SerializeField]
    private float explosionRange = 0f;
    [SerializeField]
    private LayerMask explosionLayer;
}
