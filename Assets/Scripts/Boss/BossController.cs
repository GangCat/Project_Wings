using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;
//using Cysharp.Threading.Tasks;

public class BossController : MonoBehaviour
{
    public void Init(Transform _playerTr, GameObject _gatlingHolderGo, GameObject _gatlingHeadGo,Transform _gunMuzzleTr)
    {
        curPhaseNum = 1;
        animCtrl = GetComponentInChildren<BossAnimationController>();
        bossCollider = GetComponentInChildren<BossCollider>();
        animCtrl.Init();
        bossCollider.Init();

        myRunner = GetComponent<BehaviourTreeRunner>();
        myRunner.Init(_playerTr, _gatlingHolderGo, _gatlingHeadGo, _gunMuzzleTr, animCtrl, bossCollider);

        curWeakPoint = new List<GameObject>();
        waitFixedUpdate = new WaitForFixedUpdate();

        InitNewWeakPoint();



        StartCoroutine("UpdateCoroutine");
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
        // 연출 시작

        Invoke("FinishPhaseChange", 5f); // 테스트용
    }

    private void FinishPhaseChange()
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
                foreach (Transform tr in arrFirstPhaseWeakPointTr)
                {
                    curWeakPoint.Add(Instantiate(bossWeakPointPrefab, tr.position, Quaternion.identity));
                }
                break;
            case 2:
                foreach (Transform tr in arrSecondPhaseWeakPointTr)
                {
                    curWeakPoint.Add(Instantiate(bossWeakPointPrefab, tr.position, Quaternion.identity));
                }
                break;
            case 3:
                curWeakPoint.Add(Instantiate(bossWeakPointPrefab, thirdPhaseWeakPointTr.position, Quaternion.identity));
                break;
            default:
                break;
        }

        foreach (GameObject go in curWeakPoint)
            go.GetComponent<BossWeakPoint>().Init(RemoveWeakPointFromList);
    }

    private void RemoveWeakPointFromList(GameObject _go)
    {
        curWeakPoint.Remove(_go);
    }

    [SerializeField]
    private Transform[] arrFirstPhaseWeakPointTr = null;
    [SerializeField]
    private Transform[] arrSecondPhaseWeakPointTr = null;
    [SerializeField]
    private Transform thirdPhaseWeakPointTr = null;
    [SerializeField]
    private GameObject bossWeakPointPrefab = null;

    private BossCollider bossCollider = null;
    private List<GameObject> curWeakPoint = null;
    private BehaviourTreeRunner myRunner = null;
    private int curPhaseNum = 0;
    private bool isChangingPhase = false;

    private WaitForFixedUpdate waitFixedUpdate = null;
    private BossAnimationController animCtrl = null;
}
