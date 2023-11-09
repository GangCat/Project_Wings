using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;
//using Cysharp.Threading.Tasks;

public class BossController : MonoBehaviour
{
    public void Init(Transform _playerTr, VoidIntDelegate _cameraActionCallback)
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
        statHp.Init(StartPhaseChange);
        shield.Init();
        timeBombPatternCtrl.Init(FinishPhaseChange, () => { isBossStartRotation = true; });

        myRunner = GetComponent<BehaviourTreeRunner>();
        myRunner.Init(_playerTr, gatlingHolderGo, gatlingHeadGo, gunMuzzleTr, animCtrl, bossCollider, secondShieldGeneratorSpawnPointHolder, giantHomingMissilePrefab, giantHomingMissileSpawnTr, arrGroupHomingMissileSpawnPos);

        curWeakPoint = new List<GameObject>();
        waitFixedUpdate = new WaitForFixedUpdate();

        InitNewWeakPoint();

        cameraActionCallback = _cameraActionCallback;
        playerTr = _playerTr;
        //myRunner.FinishCurrentPhase();
        //StartPhaseChange();

        //StartCoroutine("UpdateCoroutine");
    }

    public void GameStart()
    {
        myRunner.FinishCurrentPhase();
        StartPhaseChange();

        StartCoroutine("UpdateCoroutine");
    }

    public void ClearCurPhase()
    {
        foreach (GameObject bwpGo in curWeakPoint)
            Destroy(bwpGo);

        curWeakPoint.Clear();
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if (isBossStartRotation)
                RotateToTarget();

            myRunner.RunnerUpdate();
            if (!IsWeakPointRemain() && !isChangingPhase)
            {
                myRunner.FinishCurrentPhase();
                StartPhaseChange();
            }
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
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = targetRotation;
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
        isChangingPhase = true;
        cameraActionCallback?.Invoke(curPhaseNum);

        // ���� ����
        if(curPhaseNum == 1)
        {

            timeBombPatternCtrl.StartPattern();
        }

        // ���� ����

        //Invoke("FinishPhaseChange", 5f); // �׽�Ʈ��
    }

    public void FinishPhaseChange()
    {
        // ���� ����� ȣ��
        if (curPhaseNum < 3 && isChangingPhase)
        {
            isBossStartRotation = false;
            ++curPhaseNum;
            InitNewWeakPoint();
            myRunner.StartNextPhase(curPhaseNum);
            isChangingPhase = false;
        }
    }

    private bool IsWeakPointRemain()
    {
        return curWeakPoint.Count > 0;
    }

    private void InitNewWeakPoint()
    {
        curWeakPoint.Clear();

        switch (curPhaseNum)
        {
            case 1:
                firstShieldGeneratorSpawnPointHolder.Init();
                foreach (BossShieldGeneratorSpawnPoint wp in firstShieldGeneratorSpawnPointHolder.ShieldGeneratorSpawnPoints)
                {
                    curWeakPoint.Add(Instantiate(bossShieldGeneratorPrefab, wp.GetPos(), Quaternion.identity));
                }
                break;
            case 2:
                secondShieldGeneratorSpawnPointHolder.Init();
                foreach (BossShieldGeneratorSpawnPoint wp in secondShieldGeneratorSpawnPointHolder.ShieldGeneratorSpawnPoints)
                {
                    wp.Init();
                    curWeakPoint.Add(Instantiate(bossShieldGeneratorPrefab, wp.GetPos(), Quaternion.identity));
                }
                break;
            case 3:
                curWeakPoint.Add(Instantiate(bossShieldGeneratorPrefab, thirdShieldGeneratorSpawnPoint.GetPos(), Quaternion.identity, thirdShieldGeneratorSpawnPoint.transform));
                break;
            default:
                break;


        }

        foreach (GameObject go in curWeakPoint)
        {
            go.GetComponent<BossShieldGenerator>().Init(RemoveWeakPointFromList);
        }
    }

    private void RemoveWeakPointFromList(GameObject _go)
    {
        curWeakPoint.Remove(_go);
    }

    [Header("-InformationForContext")]
    [SerializeField]
    private BossShieldGeneratorSpawnPointHolder firstShieldGeneratorSpawnPointHolder = null;
    [SerializeField]
    private BossShieldGeneratorSpawnPointHolder secondShieldGeneratorSpawnPointHolder = null;
    [SerializeField]
    private BossShieldGeneratorSpawnPoint thirdShieldGeneratorSpawnPoint = null;
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

    [Header("-InformationForBossController")]
    [SerializeField]
    private GameObject bossShieldGeneratorPrefab = null;
    [SerializeField]
    private float rotationSpeed = 20f;

    private Transform playerTr = null;
    private BossCollider bossCollider = null;
    private List<GameObject> curWeakPoint = null;
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
