using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
//using Cysharp.Threading.Tasks;

public class BossController : MonoBehaviour
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
        // 플레이어의 위치와 보스의 위치 사이의 벡터를 계산
        Vector3 directionToPlayer = playerTr.position - transform.position;

        // y축 회전을 제외한 방향 벡터를 얻기 위해 y값을 0으로 만듭니다.
        directionToPlayer.y = 0f;

        // 방향 벡터를 사용하여 보스를 회전시킵니다.
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

        //    // 부드럽게 회전하기 위해 Lerp 사용
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //}
    }

    private void StartPhaseChange()
    {
        if (isChangingPhase)
            return;

        isChangingPhase = true;
        cameraActionCallback?.Invoke(curPhaseNum);

        // 패턴 시작
        if(curPhaseNum == 1)
        {
            timeBombPatternCtrl.StartPattern();
            StartCoroutine("UpdatePatternCoroutine");
        }

        // 연출 시작

        //Invoke("FinishPhaseChange", 5f); // 테스트용
    }

    private IEnumerator UpdatePatternCoroutine()
    {
        while (true)
        {
            if (isBossStartRotation)
                RotateToTarget();
        }
    }

    public void FinishPhaseChange()
    {
        // 연출 종료시 호출
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
