using System;
using System.Collections;
using UnityEngine;

public class GroupHomingMissile : AttackableObject, IDamageable, ISubscriber
{
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRange = 0f;
    [SerializeField] private LayerMask explosionLayer;

    [Header("MOVEMENT")]
    [SerializeField] private float speed = 15;
    [SerializeField] private float rotateSpeed = 95;

    [Header("PREDICTION")]
    [SerializeField] private float maxDistancePredict = 100;
    [SerializeField] private float minDistancePredict = 5;
    [SerializeField] private float maxTimePrediction = 5;
    private Vector3 standardPrediction, deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] private float deviationAmount = 50;
    [SerializeField] private float deviationSpeed = 2;

    private bool isPhaseChanged = false;
    private bool isShieldBreak = false;
    private bool isBodyTrigger = true;
    private bool isExplosed = false;
    private GroupMissileMemoryPool groupMissileMemoryPool = null;


    public float GetCurHp => throw new NotImplementedException();

    public void Init(GameObject _target, float _speed, float _rotateSpeed, Vector3 _spawnPos, Quaternion _spawnRot, GroupMissileMemoryPool _groupMissileMemoryPool, bool _isShieldBreak)
    {
        target = _target;
        speed = _speed;
        rotateSpeed = _rotateSpeed;
        groupMissileMemoryPool = _groupMissileMemoryPool;
        transform.position = _spawnPos;
        transform.rotation = _spawnRot;

        isPhaseChanged = false;
        isBodyTrigger = true;
        isExplosed = false;
        isShieldBreak = _isShieldBreak;
        if (isShieldBreak)
            isFirstTrigger = false;
        else
            isFirstTrigger = true;

        deviationAmount = UnityEngine.Random.Range(5f, 20f);
        deviationSpeed = UnityEngine.Random.Range(0.1f, 1f);

        Subscribe();
        StartCoroutine("FixedUpdateCoroutine");
    }

    private IEnumerator FixedUpdateCoroutine()
    {
        while (true)
        {
            if (isPhaseChanged)
            {
                groupMissileMemoryPool.DeactivateGroupMissile(gameObject);
                yield break;
            }

            rb.velocity = transform.forward * speed;

            var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));

            PredictMovement(leadTimePercentage);
            AddDeviation(leadTimePercentage);

            RotateRocket();

            yield return new WaitForFixedUpdate();
        }
    }

    private void PredictMovement(float _leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, maxTimePrediction, _leadTimePercentage);

        standardPrediction = target.transform.position + target.GetComponent<Rigidbody>().velocity * predictionTime;
    }

    private void AddDeviation(float _leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * _leadTimePercentage;

        deviatedPrediction = standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (!isShieldBreak && isFirstTrigger)
            return;

        else if (isBodyTrigger && _other.gameObject.layer == LayerMask.NameToLayer("BossBody"))
            return;

        Explosion();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BossShield") && isShieldBreak)
            Explosion();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BossShield"))
            isFirstTrigger = false;
        else if (other.CompareTag("BossBody"))
            isBodyTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explosion();
    }

    public void GetDamage(float _dmg)
    {
        Explosion();
    }

    public void Explosion()
    {
        if (isExplosed)
            return;

        //StopCoroutine("AutoExplosionCorutine");
        isExplosed = true;
        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        go.transform.localScale = Vector3.one * explosionRange;
        Destroy(go, 5f);

        Collider[] arrTempCollider = Physics.OverlapSphere(transform.position, explosionRange, explosionLayer);
        foreach (Collider col in arrTempCollider)
        {
            Debug.Log(col.name);
            AttackDmg(col);
        }
        groupMissileMemoryPool.DeactivateGroupMissile(gameObject);
    }

    private void OnDisable()
    {
        Broker.UnSubscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void ReceiveMessage(EMessageType _message)
    {
        switch (_message)
        {
            case EMessageType.PHASE_CHANGE:
                isPhaseChanged = true;
                break;
            case EMessageType.SHIELD_BROKEN:
                isShieldBreak = true;
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, standardPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(standardPrediction, deviatedPrediction);
    }
}
