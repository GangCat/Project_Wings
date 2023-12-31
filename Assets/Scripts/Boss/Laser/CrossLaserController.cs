using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossLaserController : AttackableObject
{
    public void Init(float _moveAccel, float _maxMoveSpeed, float _rotateAccel, float _maxRotateAccel, float _ChangeFormDistance, Transform _targetTr, float _autoDestroyTime)
    {
        moveAccel = _moveAccel;
        maxMoveSpeed = _maxMoveSpeed;
        rotateAccel = _rotateAccel;
        maxRotateSpeed = _maxRotateAccel;
        changeFormDistance = _ChangeFormDistance;
        targetTr = _targetTr;
        waitFixed = new WaitForFixedUpdate();
        moveSpeed = 0f;
        rotateSpeed = 0f;

        laserObject.SetActive(false);
        sphereObject.SetActive(true);
        Destroy(gameObject, _autoDestroyTime);

        StartCoroutine(MoveUpdateCoroutine());
    }


    private IEnumerator MoveUpdateCoroutine()
    {
        Vector3 targetDistance = Vector3.zero;

        while (true)
        {
            targetDistance = targetTr.position - transform.position;
            if (!isTargetInRange && Vector3.SqrMagnitude(targetDistance) < Mathf.Pow(changeFormDistance, 2f))
            {
                isTargetInRange = true;
                StartCoroutine(ChangeFormCoroutine());
            }

            MoveCrossLaser();

            if (isFormChange || isPathBlock())
            {
                yield return waitFixed;
                continue;
            }

            RotateCrossLaser(targetDistance.normalized);

            yield return waitFixed;
        }
    }

    private bool isPathBlock()
    {
        return Physics.Linecast(transform.position, transform.forward * 50f, hitLayers);
    }

    private void MoveCrossLaser()
    {
        moveSpeed += moveAccel * Time.deltaTime;
        moveSpeed = Mathf.Min(moveSpeed, maxMoveSpeed);

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        Debug.DrawRay(transform.position, transform.forward * 100f, Color.red);
    }

    private void RotateCrossLaser(Vector3 _moveDir)
    {
        rotateSpeed += rotateAccel * Time.deltaTime;
        rotateSpeed = Mathf.Min(rotateSpeed, maxRotateSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_moveDir), rotateSpeed * Time.deltaTime);
    }

    private IEnumerator ChangeFormCoroutine()
    {
        float startTime = Time.time;
        laserObject.SetActive(true);
        isFormChange = true;
        while (Time.time - startTime <= 0.05)
        {
            sphereObject.transform.localScale -= Vector3.one * 1 / 3;
            laserObject.transform.localScale += Vector3.one * 1 / 3;

            yield return waitFixed;
        }
        sphereObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider _other)
    {
        AttackDmg(_other);
    }


    private WaitForFixedUpdate waitFixed = null;
    private float moveAccel = 0f;
    private float moveSpeed = 0f;
    private float maxMoveSpeed = 0f;
    private float rotateAccel = 0f;
    private float rotateSpeed = 0f;
    private float maxRotateSpeed = 0f;
    private float changeFormDistance = 0f;
    private Transform targetTr = null;
    private bool isTargetInRange = false;
    private bool isFormChange = false;
    private Vector3 moveDir = Vector3.zero;


    [SerializeField]
    private GameObject laserObject = null;
    [SerializeField]
    private GameObject sphereObject = null;
    [SerializeField]
    private LayerMask hitLayers;
}
