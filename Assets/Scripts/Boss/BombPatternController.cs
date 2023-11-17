using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombPatternController : MonoBehaviour
{
    public void Init(
        VoidVoidDelegate _patternFinishDelegate, 
        VoidBoolDelegate _bossRotationCallback, 
        VoidVoidDelegate _reloadCannonCallback,
        Transform _targetTr)
    {
        patternFinishCallback = _patternFinishDelegate;
        bossRotationCallback = _bossRotationCallback;
        reloadCannonCallback = _reloadCannonCallback;
        targetTr = _targetTr;

        windBlowHolder.Init();
        arrBombGo = new GameObject[4];
        waitFixedTime = new WaitForFixedUpdate();

    }

    public void StartPattern()
    {
        StartWindBlow();
        StartCoroutine(PatternCoroutine());
        Debug.Log("PatterStart");
        //Invoke("FinishPattern", 5f);
    }

    private void StartWindBlow()
    {
        WindBlowPoint[] arrWindBlowPoints = windBlowHolder.WindBlowPoints;

        foreach(WindBlowPoint wbp in arrWindBlowPoints)
        {
            wbp.StartGenerateSecond(windBlowCylinderPrefab);
        }    
    }

    private void FinishPattern()
    {
        WindBlowPoint[] arrWindBlowPoints = windBlowHolder.WindBlowPoints;

        foreach (WindBlowPoint wbp in arrWindBlowPoints)
        {
            wbp.FinishGenerate();
        }

        Debug.Log("PatterFinish");
        patternFinishCallback?.Invoke();
    }

    private IEnumerator PatternCoroutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // 일어나는 애니메이션
            // 애니메이션 종료되면 탈출
            yield return waitFixedTime;
            break;
        }

        float spawnTime = Time.time;
        int bombIdx = 0;
        while (bombIdx < 4)
        {
            if (Time.time - spawnTime > spawnBombDelay)
            {
                //점착폭탄 발사 사운드 재생
                GameObject bombGo = Instantiate(timeBombPrefab, timeBombSpawnTr.position, Quaternion.identity);
                bombGo.GetComponent<TimeBomb>().Init(timeBombDestTr[bombIdx].position, launchAngle, gravity, targetTr, colors[bombIdx]);
                arrBombGo[bombIdx] = bombGo;
                ++bombIdx;
                spawnTime = Time.time;
                reloadCannonCallback?.Invoke();
            }

            yield return waitFixedTime;
        }

        // 시한 폭탄이 다 떨어지기까지 기다리는 겸 플레이어 확인하라는 의미의 대기시간
        yield return new WaitForSeconds(4f);
        bossRotationCallback?.Invoke(true);

        float laserStartTime = Time.time;
        int laserCount = 0;
        GameObject laserGo = null;
        Debug.Log("StartLaserCharge");
        //레이저 기모으는 사운드 재생(루프)


        for(int i = 0; i < arrBombGo.Length; ++i)
            arrBombGo[i].GetComponent<TimeBomb>().StartTimer((i + 1) * 12);

        while (true)
        {
            if (Time.time - laserStartTime > laserDelay - 1f)
            {
                //레이저 기모으는 사운드 정지
                bossRotationCallback?.Invoke(false);
                Quaternion laserRotation = CalcLaserRotation();

                while (Time.time - laserStartTime < laserDelay)
                    yield return waitFixedTime;

                laserGo = LaunchLaser(laserRotation, colors[laserCount]);
                ++laserCount;
            }

            if (laserGo)
            {
                while (laserGo)
                    yield return waitFixedTime;

                if(laserCount >= 4)
                    break;

                //레이저 기모으는 사운드 재생(루프)
                Debug.Log("StartLaserCharge");
                laserStartTime = Time.time;
                bossRotationCallback?.Invoke(true);
            }

            yield return waitFixedTime;
        }

        bossRotationCallback?.Invoke(false);
        bool isPatternFinish = false;
        while (true)
        {
            isPatternFinish = true;
            foreach(GameObject go in arrBombGo)
            {
                if (go != null)
                {
                    isPatternFinish = false;
                    break;
                }
            }

            if (isPatternFinish)
                break;

            yield return waitFixedTime;
        }

        FinishPattern();
    }

    private GameObject LaunchLaser(Quaternion _laserRotation, Color _curColor)
    {

        GameObject laserGo = Instantiate(laserPrefab, laserLaunchTr.position, laserLaunchTr.rotation * _laserRotation);
        laserGo.GetComponent<LaserController>().Init(laserDuration, laserLengthPerSec,
            _value =>
        {
            foreach (GameObject go in arrBombGo)
            {
                if (!go.Equals(_value))
                    continue;

                Destroy(go);
            }
        }, initWidth, initHeight, _curColor);

        Destroy(laserGo, laserDuration);
        return laserGo;
    }

    private Quaternion CalcLaserRotation()
    {
        float angleToPlayer = Mathf.Asin((targetTr.position.y - laserLaunchTr.position.y) / Vector3.Distance(laserLaunchTr.position, targetTr.position));
        angleToPlayer = Mathf.Clamp(angleToPlayer * Mathf.Rad2Deg, -launchAngleLimit, launchAngleLimit);
        return Quaternion.Euler(Vector3.left * angleToPlayer);
    }


    [Header("-TimeBomb")]
    [SerializeField]
    private GameObject timeBombPrefab = null;
    [SerializeField]
    private Transform[] timeBombDestTr = null;
    [SerializeField]
    private Transform timeBombSpawnTr = null;
    [SerializeField]
    private float spawnBombDelay = 0f; // 폭탄 4개 각각 생성하는 딜레이
    [SerializeField]
    private float launchAngle = 45f;
    [SerializeField]
    private float gravity = 9.81f;
    [SerializeField]
    [ColorUsage(true,true)]
    private Color[] colors = null;


    [Header("-Laser")]
    [SerializeField]
    private GameObject laserPrefab = null;
    [SerializeField]
    private Transform laserLaunchTr = null;
    [SerializeField]
    private float laserDelay = 0f;
    [SerializeField]
    private float laserDuration = 0f;
    [SerializeField]
    private float laserLengthPerSec = 0f;
    [SerializeField]
    private float initWidth = 20f;
    [SerializeField]
    private float initHeight = 20f;
    [SerializeField]
    private float launchAngleLimit = 30f;

    [Header("-E.T.C")]
    [SerializeField]
    private GameObject windBlowCylinderPrefab = null;
    [SerializeField]
    private float startDelay = 10f;
    [SerializeField]
    private WindBlowHolder windBlowHolder = null;

    private GameObject[] arrBombGo = null;
    private VoidVoidDelegate patternFinishCallback = null;
    private VoidBoolDelegate bossRotationCallback = null;
    private VoidVoidDelegate reloadCannonCallback = null;
    private WaitForFixedUpdate waitFixedTime = null;
    private Transform targetTr = null;

    private float curLaserLength = 0f;
}
