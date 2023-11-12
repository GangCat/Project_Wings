using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantHomingMissileController : AttackableObject, IDamageable, ISubscriber
{
    public void Init(
        float _moveAccel,
        float _maxMoveSpeed,
        float _rotateAccel,
        float _maxRotateAccel,
        Transform _targetTr,
        float _autoDestroyTime,
        bool _isShieldBreak,
        GroupMissileMemoryPool _groupMissileMemoryPool = null
        )
    {
        moveAccel = _moveAccel;
        maxMoveSpeed = _maxMoveSpeed;
        rotateAccel = _rotateAccel;
        maxRotateSpeed = _maxRotateAccel;
        targetTr = _targetTr;
        waitFixed = new WaitForFixedUpdate();
        moveSpeed = maxMoveSpeed;
        rotateSpeed = 0f;
        isShieldBreak = _isShieldBreak;
        groupMissileMemoryPool = _groupMissileMemoryPool;

        isFirstTrigger = true;
        isExplosed = false;
        isPhaseChange = false;

        Subscribe();
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

            if (isPhaseChange)
                Destroy(gameObject);
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
        if (!isShieldBreak && isFirstTrigger)
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
        GameObject go = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        go.transform.localScale = Vector3.one * explosionRange;
        Destroy(go, 5f);

        Collider[] arrTempCollider = Physics.OverlapSphere(transform.position, explosionRange, explosionLayer);
        foreach(Collider col in arrTempCollider)
        {
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

    public void GetDamage(float _dmg)
    {
        Explosion();
    }

    private void OnDestroy()
    {
        Broker.UnSubscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void ReceiveMessage(EMessageType _message)
    {
        if (_message == EMessageType.PHASE_CHANGE)
            isPhaseChange = true;
    }

    private GroupMissileMemoryPool groupMissileMemoryPool = null;
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
    private bool isShieldBreak = false;
    private bool isPhaseChange = false;

    [SerializeField]
    private GameObject explosionEffectPrefab;
    [SerializeField]
    private float explosionRange = 0f;
    [SerializeField]
    private LayerMask explosionLayer;

    public float GetCurHp => throw new System.NotImplementedException();
}
