using System;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;
//using Cysharp.Threading.Tasks;

public class BossController : MonoBehaviour, IPublisher
{
    public delegate BossShieldGeneratorSpawnPoint[] GetRandomSpawnPointDelegate();
    public void Init(Transform _playerTr, VoidIntDelegate _cameraActionCallback, VoidFloatDelegate _hpUpdateCallback, VoidFloatDelegate _shieldUpdateCallback, GetRandomSpawnPointDelegate _getRandomSpawnPointCallback, VoidVoidDelegate _bossClearCalblack, VoidVoidDelegate _removeShieldCallback)
    {
        curPhaseNum = 0;
        animCtrl = GetComponentInChildren<BossAnimationController>();
        bossCollider = GetComponentInChildren<BossCollider>();
        statHp = GetComponent<BossStatusHp>();
        shield = GetComponentInChildren<BossShield>();
        timeBombPatternCtrl = GetComponentInChildren<TimeBombPatternController>();
        bossRb = GetComponent<Rigidbody>();
        lastPatternCtrl = GetComponentInChildren<LastPatternController>();

        getRandomSpawnPointCallback = _getRandomSpawnPointCallback;

        animCtrl.Init();
        bossCollider.Init();
        statHp.Init(StartPhaseChange, _hpUpdateCallback);
        shield.Init(RestorShieldFinish, _shieldUpdateCallback, _removeShieldCallback);
        lastPatternCtrl.Init(_bossClearCalblack);
        timeBombPatternCtrl.Init(FinishPhaseChange, value => { isBossStartRotation = value; }, _playerTr);
        RegisterBroker();
        InitMemoryPools();

        myRunner = GetComponent<BehaviourTreeRunner>();
        myRunner.Init(_playerTr, animCtrl, bossCollider, giantHomingMissileSpawnTr, arrGroupHomingMissileSpawnPos, this);

        curShieldGeneratorPoint = new List<GameObject>();
        waitFixedUpdate = new WaitForFixedUpdate();

        //InitShieldGeneratorPoint();

        cameraActionCallback = _cameraActionCallback;
        playerTr = _playerTr;
        //myRunner.FinishCurrentPhase();
        //StartPhaseChange();

        //StartCoroutine("UpdateCoroutine");
    }

    public BossShieldGeneratorSpawnPoint[] CurSpawnPoints => arrCurShieldGeneratorSpawnPoints;
    public GameObject GatlingHolder => gatlingHolderGo;
    public GameObject GatlingHead => gatlingHeadGo;
    public GameObject AirPush => airPush;
    public Transform GunMuzzle => gunMuzzleTr;
    public CannonMemoryPool CannonMemoryPool => cannonMemoryPool;
    public GroupMissileMemoryPool GroupMissileMemoryPool => groupMissileMemoryPool;
    public GatlinMemoryPool GatlinMemoryPool => gatlinMemoryPool;
    public CannonRainMemoryPool CannonRainMemoryPool => cannonRainMemoryPool;
    public Transform[] FootWindTr => arrFootWindSpawnTr;

    public GameObject FootWindGo => footWindGo;

    public GameObject SitDownGo => sitDownGo;

    public void SetBossRotationBoolean(bool _canRotation)
    {
        isBossStartRotation = _canRotation;
    }

    public void GameStart()
    {
        myRunner.FinishCurrentPhase();
        StartPhaseChange();

        StartCoroutine("UpdateCoroutine");
    }

    public void ClearShieldGenerator()
    {
        int curGenCnt = curShieldGeneratorPoint.Count;
        for (int i = 0; i < curGenCnt; ++i)
            curShieldGeneratorPoint[0].GetComponent<IDamageable>().GetDamage(999);

        //foreach (GameObject go in curShieldGeneratorPoint)
        //    go.GetComponent<BossShieldGenerator>().GetDamage(999);
    }

    public void JumpToNextPhase()
    {
        ClearShieldGenerator();
        statHp.GetDamage(statHp.GetMaxHp * 0.51f);
    }

    private void InitMemoryPools()
    {
        cannonRainMemoryPool.Init();
        cannonMemoryPool.Init();
        gatlinMemoryPool.Init();
        groupMissileMemoryPool.Init();

    }

    private IEnumerator UpdateCoroutine()
    {
        float maxHp = statHp.GetMaxHp;

        while (true)
        {
            if (isBossStartRotation)
                RotateToTarget();

            myRunner.RunnerUpdate();
            yield return waitFixedUpdate;
        }
    }

    private void RotateToTarget()
    {
        // �÷��̾��� ��ġ�� ������ ��ġ ������ ���͸� ���
        Vector3 directionToPlayer = playerTr.position - transform.position;

        // y�� ȸ���� ������ ���� ���͸� ��� ���� y���� 0���� ����ϴ�.
        directionToPlayer.y = 0f;

        // ���� ���͸� ����Ͽ� ������ ȸ����ŵ�ϴ�.
        if (curPhaseNum >= 2)
            transform.localRotation *= Quaternion.Euler(new Vector3(0f, autoRotateDegree * Time.deltaTime, 0f));
        else if (directionToPlayer != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), rotationSpeed * Time.fixedDeltaTime);

    }

    private void StartPhaseChange()
    {
        if (isChangingPhase)
            return;

        myRunner.FinishCurrentPhase();
        myRunner.RunnerUpdate();
        isChangingPhase = true;
        StopCoroutine("ResapwnShieldGeneratorCoroutine");
        foreach (GameObject go in arrModelGo)
            go.layer = LayerMask.NameToLayer("BossInvincible");
        cameraActionCallback?.Invoke(curPhaseNum);
        PushMessageToBroker(EMessageType.PHASE_CHANGE);

        if (curPhaseNum == 1)
        {
            shield.StopRestorShield();
            StartCoroutine(TempBossUP());
        }
    }

    IEnumerator TempBossUP()
    {
        float t = 0f;
        float oriY = transform.position.y; // 60
        float targetY = 430f;
        float startTime = Time.time;
        Vector3 oriPos = transform.position;
        Vector3 targetPos = new Vector3(oriPos.x, targetY, oriPos.z);
        while (t < 1)
        {
            t = (Time.time - startTime) / 3.5f;
            transform.position = Vector3.Lerp(oriPos, targetPos, t);

            yield return new WaitForFixedUpdate();
        }
    }

    public void PatternStart()
    {
        // ���� ����� ȣ��
        if (!isChangingPhase)
            return;

        if (curPhaseNum == 1)
        {
            timeBombPatternCtrl.StartPattern();
            return;
        }
        
        if (curPhaseNum >= 2)
        {
            playerTr.GetComponent<PlayerMovementController>().IsLastPattern = true;
            lastPatternCtrl.StartPattern();
        }

        FinishPhaseChange();
    }


    public void FinishPhaseChange()
    {
        if (!isChangingPhase)
            return;

        isBossStartRotation = false;
        isChangingPhase = false;

        if (curPhaseNum == 0)
        {
            InitShieldGeneratorPoint();
            shield.RespawnGenerator();
        }
        else if (curPhaseNum == 1)
        {
            foreach (GameObject go in arrModelGo)
                go.layer = LayerMask.NameToLayer("BossBody");

            foreach (GameObject go in coreGo)
                go.layer = LayerMask.NameToLayer("Boss");
        }
        else if(curPhaseNum >= 2)
        {
            foreach (GameObject go in arrModelGo)
                go.layer = LayerMask.NameToLayer("BossBody");

            isBossStartRotation = true;
        }

        ++curPhaseNum;
        myRunner.StartNextPhase(curPhaseNum);
    }

    private void RestorShieldFinish()
    {
        InitShieldGeneratorPoint();
        myRunner.IsShieldDestroy(false);
    }

    private void InitShieldGeneratorPoint()
    {
        curShieldGeneratorPoint.Clear();
        arrCurShieldGeneratorSpawnPoints = getRandomSpawnPointCallback?.Invoke();

        foreach (BossShieldGeneratorSpawnPoint wp in arrCurShieldGeneratorSpawnPoints)
        {
            wp.Init();
            curShieldGeneratorPoint.Add(Instantiate(bossShieldGeneratorPrefab, wp.GetPos(), Quaternion.identity));
        }

        foreach (GameObject go in curShieldGeneratorPoint)
        {
            go.GetComponent<BossShieldGenerator>().Init(RemoveSheildGeneratorFromList);
        }
    }

    private void RemoveSheildGeneratorFromList(GameObject _go)
    {
        curShieldGeneratorPoint.Remove(_go);

        if (curShieldGeneratorPoint.Count < 1)
        {
            PushMessageToBroker(EMessageType.SHIELD_BROKEN);
            foreach (GameObject go in arrModelGo)
                go.layer = LayerMask.NameToLayer("Boss");
            myRunner.IsShieldDestroy(true);
        }

        shield.GeneratorDestroy();
    }

    public void RegisterBroker()
    {
        Broker.Regist(EPublisherType.BOSS_CONTROLLER);
    }

    public void PushMessageToBroker(EMessageType _message)
    {
        Broker.AlertMessageToSub(_message, EPublisherType.BOSS_CONTROLLER);
    }

    [Header("-InformationForContext")]
    [SerializeField]
    private GameObject gatlingHolderGo = null;
    [SerializeField]
    private GameObject gatlingHeadGo = null;
    [SerializeField]
    private Transform gunMuzzleTr = null;
    [SerializeField]
    private Transform giantHomingMissileSpawnTr = null;
    [SerializeField]
    private GameObject giantHomingMissilePrefab = null;
    [SerializeField]
    private GroupHomingMissileSpawnPos[] arrGroupHomingMissileSpawnPos = null;
    [SerializeField]
    private CannonRainMemoryPool cannonRainMemoryPool = null;
    [SerializeField]
    private CannonMemoryPool cannonMemoryPool = null;
    [SerializeField]
    private GatlinMemoryPool gatlinMemoryPool = null;
    [SerializeField]
    private GroupMissileMemoryPool groupMissileMemoryPool = null;
    [SerializeField]
    private GameObject airPush= null;
    [SerializeField]
    private Transform[] arrFootWindSpawnTr = null;
    [SerializeField]
    private GameObject footWindGo = null;
    [SerializeField]
    private GameObject sitDownGo = null;

    [Header("-InformationForBossController")]
    [SerializeField]
    private GameObject bossShieldGeneratorPrefab = null;
    [SerializeField]
    private float rotationSpeed = 20f;
    [SerializeField]
    private float respawnShieldGeneratorTime = 0f;
    [SerializeField]
    private GameObject[] arrModelGo = null;
    [SerializeField]
    private GameObject[] coreGo = null;
    [SerializeField]
    private float autoRotateDegree = 30f;


    private Transform playerTr = null;
    private BossCollider bossCollider = null;
    private List<GameObject> curShieldGeneratorPoint = null;
    private BehaviourTreeRunner myRunner = null;
    private int curPhaseNum = 0;
    private bool isChangingPhase = false;
    private Rigidbody bossRb = null;

    private WaitForFixedUpdate waitFixedUpdate = null;
    private BossAnimationController animCtrl = null;
    private VoidIntDelegate cameraActionCallback = null;
    private BossStatusHp statHp = null;
    private BossShield shield = null;
    private TimeBombPatternController timeBombPatternCtrl = null;
    private GetRandomSpawnPointDelegate getRandomSpawnPointCallback = null;
    private BossShieldGeneratorSpawnPoint[] arrCurShieldGeneratorSpawnPoints = null;
    private LastPatternController lastPatternCtrl = null;

    private bool isBossStartRotation = false;
}
