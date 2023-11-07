using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;
//using Cysharp.Threading.Tasks;

public class BossController : MonoBehaviour
{
    public void Init(
        Transform _playerTr,
        GameObject _gatlingHolderGo,
        GameObject _gatlingHeadGo,
        Transform _gunMuzzleTr,
        GameObject giantHomingMissileGo,
        Transform _giantHomingMissileSpawnTr,
        VoidIntDelegate _cameraActionCallback)
    {
        curPhaseNum = 0;
        animCtrl = GetComponentInChildren<BossAnimationController>();
        bossCollider = GetComponentInChildren<BossCollider>();
        statHp = GetComponent<BossStatusHp>();

        animCtrl.Init();
        bossCollider.Init();
        statHp.Init(StartPhaseChange);

        myRunner = GetComponent<BehaviourTreeRunner>();
        myRunner.Init(_playerTr, _gatlingHolderGo, _gatlingHeadGo, _gunMuzzleTr, animCtrl, bossCollider, secondWeakPointHolder, giantHomingMissileGo, _giantHomingMissileSpawnTr);

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
                firstWeakPointHolder.Init();
                foreach (WeakPoint wp in firstWeakPointHolder.WeakPoints)
                {
                    curWeakPoint.Add(Instantiate(bossWeakPointPrefab, wp.GetPos(), Quaternion.identity));
                }
                break;
            case 2:
                secondWeakPointHolder.Init();
                foreach (WeakPoint wp in secondWeakPointHolder.WeakPoints)
                {
                    wp.Init();
                    curWeakPoint.Add(Instantiate(bossWeakPointPrefab, wp.GetPos(), Quaternion.identity));
                }
                break;
            case 3:
                curWeakPoint.Add(Instantiate(bossWeakPointPrefab, thirdPhaseWeakPoint.GetPos(), Quaternion.identity));
                break;
            default:
                break;
        }

        foreach (GameObject go in curWeakPoint)
        {
            go.transform.parent = transform;
            go.GetComponent<BossWeakPoint>().Init(RemoveWeakPointFromList);
        }
    }

    private void RemoveWeakPointFromList(GameObject _go)
    {
        curWeakPoint.Remove(_go);
    }

    [SerializeField]
    private WeakPointHolder firstWeakPointHolder = null;
    [SerializeField]
    private WeakPointHolder secondWeakPointHolder = null;
    [SerializeField]
    private WeakPoint thirdPhaseWeakPoint = null;
    [SerializeField]
    private GameObject bossWeakPointPrefab = null;

    private BossCollider bossCollider = null;
    private List<GameObject> curWeakPoint = null;
    private BehaviourTreeRunner myRunner = null;
    private int curPhaseNum = 0;
    private bool isChangingPhase = false;

    private WaitForFixedUpdate waitFixedUpdate = null;
    private BossAnimationController animCtrl = null;
    private VoidIntDelegate cameraActionCallback = null;
    private BossStatusHp statHp = null;
}
