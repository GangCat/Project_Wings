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

        animCtrl.Init();
        bossCollider.Init();
        statHp.Init(StartPhaseChange);
        shield.Init();

        myRunner = GetComponent<BehaviourTreeRunner>();
        myRunner.Init(_playerTr, gatlingHolderGo, gatlingHeadGo, gunMuzzleTr, animCtrl, bossCollider, secondShieldGeneratorSpawnPointHolder, giantHomingMissilePrefab, giantHomingMissileSpawnTr, arrGroupHomingMissileSpawnPos);

        curWeakPoint = new List<GameObject>();
        waitFixedUpdate = new WaitForFixedUpdate();

        InitNewWeakPoint();

        cameraActionCallback = _cameraActionCallback;
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
            myRunner.RunnerUpdate();
            if (!IsWeakPointRemain() && !isChangingPhase)
            {
                myRunner.FinishCurrentPhase();
                StartPhaseChange();
            }
            yield return waitFixedUpdate;
        }
    }

    private void StartPhaseChange()
    {
        isChangingPhase = true;
        cameraActionCallback?.Invoke(curPhaseNum);
        // 연출 시작

        //Invoke("FinishPhaseChange", 5f); // 테스트용
    }

    public void FinishPhaseChange()
    {
        // 연출 종료시 호출
        if (curPhaseNum < 3)
        {
            isChangingPhase = false;
            ++curPhaseNum;
            InitNewWeakPoint();
            myRunner.StartNextPhase(curPhaseNum);
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
                    curWeakPoint.Add(Instantiate(bossWeakPointPrefab, wp.GetPos(), Quaternion.identity, wp.transform));
                }
                break;
            case 2:
                secondShieldGeneratorSpawnPointHolder.Init();
                foreach (BossShieldGeneratorSpawnPoint wp in secondShieldGeneratorSpawnPointHolder.ShieldGeneratorSpawnPoints)
                {
                    wp.Init();
                    curWeakPoint.Add(Instantiate(bossWeakPointPrefab, wp.GetPos(), Quaternion.identity, wp.transform));
                }
                break;
            case 3:
                curWeakPoint.Add(Instantiate(bossWeakPointPrefab, thirdShieldGeneratorSpawnPoint.GetPos(), Quaternion.identity, thirdShieldGeneratorSpawnPoint.transform));
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

    [SerializeField]
    private BossShieldGeneratorSpawnPointHolder firstShieldGeneratorSpawnPointHolder = null;
    [SerializeField]
    private BossShieldGeneratorSpawnPointHolder secondShieldGeneratorSpawnPointHolder = null;
    [SerializeField]
    private BossShieldGeneratorSpawnPoint thirdShieldGeneratorSpawnPoint = null;
    [SerializeField]
    private GameObject bossWeakPointPrefab = null;
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


    private BossCollider bossCollider = null;
    private List<GameObject> curWeakPoint = null;
    private BehaviourTreeRunner myRunner = null;
    private int curPhaseNum = 0;
    private bool isChangingPhase = false;

    private WaitForFixedUpdate waitFixedUpdate = null;
    private BossAnimationController animCtrl = null;
    private VoidIntDelegate cameraActionCallback = null;
    private BossStatusHp statHp = null;
    private BossShield shield = null;
}
