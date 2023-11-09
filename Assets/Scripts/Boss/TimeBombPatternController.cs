using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeBombPatternController : MonoBehaviour
{
    public void Init(VoidVoidDelegate _patternFinishDelegate, VoidVoidDelegate _startBossRotationCallback)
    {
        patternFinishCallback = _patternFinishDelegate;
        startBossRotationCallback = _startBossRotationCallback;
        arrBombGo = new GameObject[4];
        waitFixedTime = new WaitForFixedUpdate();
    }

    public void StartPattern()
    {
        StartCoroutine(PatternCoroutine());
        Debug.Log("PatterStart");
        //Invoke("FinishPattern", 5f);
    }

    private void FinishPattern()
    {
        Debug.Log("PatterFinish");
        patternFinishCallback?.Invoke();
    }

    private IEnumerator PatternCoroutine()
    {
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
                GameObject bombGo = Instantiate(timeBombPrefab, timeBombSpawnTr.position, Quaternion.identity);
                bombGo.GetComponent<TimeBomb>().Init(timeBombDestTr[bombIdx].position, launchAngle, gravity, explosionTime);
                arrBombGo[bombIdx] = bombGo;
                ++bombIdx;
                spawnTime = Time.time;
            }

            yield return waitFixedTime;
        }

        startBossRotationCallback?.Invoke();

        float laserStartTime = Time.time;
        int laserCount = 0;
        while (laserCount < 4)
        {
            if (Time.time - laserStartTime > laserDelay)
            {
                LaunchLaser();
                ++laserCount;
                laserStartTime = Time.time;
            }

            yield return waitFixedTime;
        }

        while (true)
        {
            if (arrBombGo.Length < 1)
                break;

            yield return waitFixedTime;
        }

        FinishPattern();
    }

    private void LaunchLaser()
    {
        GameObject laserGo = Instantiate(laserPrefab, laserLaunchTr.position, laserLaunchTr.rotation);
        laserGo.GetComponent<LaserController>().Init(laserDuration, laserLengthPerSec,
            _value =>
        {
            foreach (GameObject go in arrBombGo)
            {
                if (!go.Equals(_value))
                    continue;

                Destroy(go);
            }
        }, initWidth, initHeight);

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
    private float explosionTime = 0f;


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

    private GameObject[] arrBombGo = null;
    private VoidVoidDelegate patternFinishCallback = null;
    private VoidVoidDelegate startBossRotationCallback = null;
    private WaitForFixedUpdate waitFixedTime = null;

    private float curLaserLength = 0f;
}
