using System;
using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] private float deviationAmount = 0;
    [SerializeField] private float deviationSpeed = 0;

    private bool isPhaseChanged = false;
    private bool isShieldBreak = false;
    private bool isBodyTrigger = true;
    private bool isExplosed = false;
    private GroupMissileMemoryPool groupMissileMemoryPool = null;
    private CustomAudioManager customAudioManager = null;
    private Transform playerTr;
    private enum EGroupMissileAudio
    {
        NONE = -1,
        NORMALEXPLOSIONSOUND,
        HOMINGMISSILEPASSINGSOUND
    }

    float dotProduct = 0f;
    float normalizedAngle = 0f;
    float mappedValue = 0f;
    private float oriSpeed = 0f;

    public float GetCurHp => throw new NotImplementedException();

    public void Init(GameObject _target, Vector3 _spawnPos, Quaternion _spawnRot, GroupMissileMemoryPool _groupMissileMemoryPool, bool _isShieldBreak)
    {
        playerTr = _playerTr;
        customAudioManager = GetComponent<CustomAudioManager>();
        target = _target;
        groupMissileMemoryPool = _groupMissileMemoryPool;
        transform.position = _spawnPos;
        transform.rotation = _spawnRot;

        oriSpeed = speed;
        isPhaseChanged = false;
        isBodyTrigger = true;
        isExplosed = false;
        isShieldBreak = _isShieldBreak;
        if (isShieldBreak)
            isFirstTrigger = false;
        else
            isFirstTrigger = true;

        //deviationAmount = UnityEngine.Random.Range(5f, 20f);
        //deviationSpeed = UnityEngine.Random.Range(0.1f, 1f);

        Subscribe();
        StartCoroutine("FixedUpdateCoroutine");
    }

    private IEnumerator FixedUpdateCoroutine()
    {
        while (true)
        {
            // 미사일의 뒤에 불꽃 점화소리(타는 소리), 플레이어와의 거리 계산, 가까울 수록 볼륨은 커진다.
            if (isPhaseChanged)
            {
                groupMissileMemoryPool.DeactivateGroupMissile(gameObject);
                yield break;
            }

            //dotProduct = Mathf.Clamp(Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized), -1f, 1f);
            //normalizedAngle = Mathf.Acos(dotProduct) / Mathf.PI;
            //mappedValue = 1f - normalizedAngle;

            //speed = oriSpeed * (mappedValue * 0.5f + 0.5f);

            rb.velocity = transform.forward * speed;
            float distanceBetweenPlayer = Vector3.Distance(gameObject.transform.position, playerTr.position);
            if(distanceBetweenPlayer < 15f)
            {
                customAudioManager.PlayAudio((int)EGroupMissileAudio.HOMINGMISSILEPASSINGSOUND,true);
            }
            else
            {
                customAudioManager.StopAllAudio();
            }
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

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("BossShield") && isShieldBreak)
    //        Explosion();
    //}

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
        customAudioManager.PlayAudio((int)EGroupMissileAudio.NORMALEXPLOSIONSOUND);
        Collider[] arrTempCollider = Physics.OverlapSphere(transform.position, explosionRange, explosionLayer);
        foreach (Collider col in arrTempCollider)
        {
            Debug.Log(col.name);
            AttackDmg(col);
        }
        groupMissileMemoryPool.DeactivateGroupMissile(gameObject);
        // 플레이어와의 거리 계산 > 가까울 수록 볼륨은 커진다 > 미사일이 폭발하는 소리 재생
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
