using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;
//using Cysharp.Threading.Tasks;

public class BossController : MonoBehaviour, IPublisher
{
    public void Init(Transform _playerTr, VoidIntDelegate _cameraActionCallback, VoidFloatDelegate _hpUpdateCallback)
    {
        curPhaseNum = 0;
        animCtrl = GetComponentInChildren<BossAnimationController>();
        bossCollider = GetComponentInChildren<BossCollider>();
        statHp = GetComponent<BossStatusHp>();
        shield = GetComponentInChildren<BossShield>();
        timeBombPatternCtrl = GetComponentInChildren<TimeBombPatternController>();
        bossRb = GetComponent<Rigidbody>();

        animCtrl.Init();
        bossCollider.Init();
        statHp.Init(StartPhaseChange, _hpUpdateCallback);
        shield.Init();
        timeBombPatternCtrl.Init(FinishPhaseChange, value => { isBossStartRotation = value; }, _playerTr);
        RegisterBroker();
        InitMemoryPools();

        myRunner = GetComponent<BehaviourTreeRunner>();
        myRunner.Init(_playerTr, gatlingHolderGo, gatlingHeadGo, gunMuzzleTr, animCtrl, bossCollider, shieldGeneratorSpawnPointHolder, giantHomingMissilePrefab, giantHomingMissileSpawnTr, arrGroupHomingMissileSpawnPos, cannonRainMemoryPool, cannonMemoryPool, gatlinMemoryPool, groupMissileMemoryPool);

        curShieldGeneratorPoint = new List<GameObject>();
        waitFixedUpdate = new WaitForFixedUpdate();

        //InitShieldGeneratorPoint();

        cameraActionCallback = _cameraActionCallback;
        playerTr = _playerTr;
        //myRunner.FinishCurrentPhase();
        //StartPhaseChange();

        //StartCoroutine("UpdateCoroutine");
    }

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

    public void ClearCurPhase()
    {
        foreach (GameObject bwpGo in curShieldGeneratorPoint)
            Destroy(bwpGo);

        curShieldGeneratorPoint.Clear();
    }

    private void InitMemoryPools()
    {
        GetComponentInChildren<CannonRainMemoryPool>().Init();
        GetComponentInChildren<CannonMemoryPool>().Init();
        GetComponentInChildren<GatlinMemoryPool>().Init();
        GetComponentInChildren<GroupMissileMemoryPool>().Init();

    }

    private IEnumerator UpdateCoroutine()
    {
        float maxHp = statHp.GetMaxHp;

        while (true)
        {
            if (isBossStartRotation)
                RotateToTarget();

            if(statHp.GetCurHp < maxHp * 0.5f)
            {
                StartPhaseChange();
                yield break;
            }

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
        if (directionToPlayer != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), rotationSpeed * Time.fixedDeltaTime);
        }

        //bossRb.MoveRotation(bossRb.rotation * Quaternion.Euler(Vector3.up * rotationSpeed * Mathf.Deg2Rad));
        //if (playerTr != null)
        //{
        //    Vector3 playerDirection = new Vector3(playerTr.position.x, transform.position.y, playerTr.position.z);
        //    Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
        //    Debug.DrawRay(transform.position, playerDirection * 1000f, Color.red);

        //    // �ε巴�� ȸ���ϱ� ���� Lerp ���
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //}
    }

    private void StartPhaseChange()
    {
        if (isChangingPhase)
            return;

        myRunner.FinishCurrentPhase();
        myRunner.RunnerUpdate();
        isChangingPhase = true;
        gameObject.layer = LayerMask.NameToLayer("BossInvincible");
        cameraActionCallback?.Invoke(curPhaseNum);
        PushMessageToBroker(EMessageType.PHASE_CHANGE);

        // ���� ����
        if (curPhaseNum == 1)
        {
            // ��� �߻�ü��� ��� ������ ������Ʈ�� ��Ȱ��ȭ(O)
            // ���� ����
            // ���� ����� ����
            timeBombPatternCtrl.StartPattern();
            StartCoroutine("UpdatePatternCoroutine");
        }
        else if(curPhaseNum == 2)
        {
            // ������ ���ƴ��� ����
            Invoke("FinishPhaseChange", 5f);
        }

        // ���� ����

        //Invoke("FinishPhaseChange", 5f); // �׽�Ʈ��
    }

    private IEnumerator UpdatePatternCoroutine()
    {
        while (true)
        {
            if (isBossStartRotation)
                RotateToTarget();

            yield return waitFixedUpdate;
        }
    }

    public void FinishPhaseChange()
    {
        // ���� ����� ȣ��
        if (curPhaseNum < 3 && isChangingPhase)
        {
            isBossStartRotation = false;
            ++curPhaseNum;
            InitShieldGeneratorPoint();
            myRunner.StartNextPhase(curPhaseNum);
            isChangingPhase = false;
            StopCoroutine("UpdatePatternCoroutine");
            StartCoroutine(UpdateCoroutine());
        }
    }

    private bool IsShieldGeneratorRemain()
    {
        return curShieldGeneratorPoint.Count > 0;
    }

    private void InitShieldGeneratorPoint()
    {
        curShieldGeneratorPoint.Clear();

        shieldGeneratorSpawnPointHolder.Init();
        foreach (BossShieldGeneratorSpawnPoint wp in shieldGeneratorSpawnPointHolder.ShieldGeneratorSpawnPoints)
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
        shield.GeneratorDestroy();
        curShieldGeneratorPoint.Remove(_go);

        if (curShieldGeneratorPoint.Count < 1)
        {
            foreach (GameObject go in arrModelGo)
                go.layer = LayerMask.NameToLayer("Boss");
            StartCoroutine("ResapwnShieldGeneratorCoroutine");
            myRunner.IsShieldDestroy(true);
        }
    }

    private IEnumerator ResapwnShieldGeneratorCoroutine()
    {
        yield return new WaitForSeconds(respawnShieldGeneratorTime);

        InitShieldGeneratorPoint();
        shield.RespawnGenerator();
        myRunner.IsShieldDestroy(false);
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
    private BossShieldGeneratorSpawnPointHolder shieldGeneratorSpawnPointHolder = null;
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

    [Header("-InformationForBossController")]
    [SerializeField]
    private GameObject bossShieldGeneratorPrefab = null;
    [SerializeField]
    private float rotationSpeed = 20f;
    [SerializeField]
    private float respawnShieldGeneratorTime = 0f;
    [SerializeField]
    private GameObject[] arrModelGo = null;

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

    private bool isBossStartRotation = false;
}
