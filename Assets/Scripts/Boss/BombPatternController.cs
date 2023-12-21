using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class BombPatternController : MonoBehaviour
{
    public delegate void VoidColorDelegate(Color _color);
    VoidColorDelegate changeBodyEyeColorCallback = null;

    public void Init(
        VoidVoidDelegate _patternFinishDelegate, 
        VoidBoolDelegate _bossRotationCallback, 
        VoidVoidDelegate _reloadCannonCallback,
        VoidVoidDelegate _alertFirstPatternCallback,
        VoidColorDelegate _changeBodyEyeColorCallback,
        Transform _targetTr)
    {
        soundManager = SoundManager.Instance;
        soundManager.Init(gameObject);
        patternFinishCallback = _patternFinishDelegate;
        bossRotationCallback = _bossRotationCallback;
        reloadCannonCallback = _reloadCannonCallback;
        alertFirstPatternCallback = _alertFirstPatternCallback;
        changeBodyEyeColorCallback = _changeBodyEyeColorCallback;
        targetTr = _targetTr;

        windBlowHolder.Init();
        arrBombGo = new GameObject[4];
        waitFixedTime = new WaitForFixedUpdate();

    }

    public void StartPattern()
    {
        if (isDebugMode)
        {
            FinishPattern();
            return;
        }

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
        // 폭탄 개수 총 4개
        while (bombIdx < 4)
        {
            if (Time.time - spawnTime > spawnBombDelay)
            {
                // 폭탄 생성
                GameObject bombGo = Instantiate(
                    timeBombPrefab, 
                    timeBombSpawnTr.position, 
                    Quaternion.identity);
                // 폭탄 스크립트 초기화
                bombGo.GetComponent<Bomb>().Init(
                    timeBombDestTr[bombIdx].position, 
                    launchAngle, 
                    gravity, 
                    targetTr, 
                    colors[bombIdx], 
                    bombIdx);
                // 배열에 추가
                arrBombGo[bombIdx] = bombGo;
                ++bombIdx;
                spawnTime = Time.time;
                // 캐논 재장전 애니메이션 출력
                reloadCannonCallback?.Invoke();
            }

            yield return waitFixedTime;
        }

        alertFirstPatternCallback?.Invoke();
        // 시한 폭탄이 다 떨어지기까지 기다리는 겸 플레이어 확인하라는 의미의 대기시간
        yield return new WaitForSeconds(4f);
        bossRotationCallback?.Invoke(true);

        // 0~3 숫자를 랜덤으로 선택해서 순서를 만들어 ranSelect배열에 저장 / 0 - 2 - 3 - 1 과 같이 저장됨
        GenerateUniqueRandomNumbers();

        float laserStartTime = Time.time;
        int laserCount = 0;
        GameObject laserGo = null;
        GameObject ChargeGo = null;
        Debug.Log("StartLaserCharge");
        changeBodyEyeColorCallback?.Invoke(colors[ranSelect[laserCount]]);
        ChargeGo = ChargeLaser(colors[ranSelect[laserCount]]);
        ChargeGo.transform.parent = laserLaunchTr;
        //레이저 기모으는 사운드 재생(루프)
        soundManager.PlayAudio(
            GetComponent<AudioSource>(), 
            (int)SoundManager.ESounds.LASERPREPERATIONSOUND,
            true);

        while (true)
        {
            if (Time.time - laserStartTime > laserDelay - 1f)
            {
                //레이저 기모으는 사운드 정지
                if (soundManager.IsPlaying(GetComponent<AudioSource>()))
                {
                    soundManager.StopAudio(GetComponent<AudioSource>());
                }
                // 보스 회전 정지
                bossRotationCallback?.Invoke(false);
                Quaternion laserRotation = CalcLaserRotation();

                // 발사 전 잠시 딜레이, 플레이어 회피 시간
                while (Time.time - laserStartTime < laserDelay)
                    yield return waitFixedTime;

                // 레이저 발사
                laserGo = LaunchLaser(
                    laserRotation, 
                    colors[ranSelect[laserCount]], 
                    ranSelect[laserCount]);
            }

            // 레이저 발사중일 경우 들어옴
            if (laserGo)
            {
                // 레이저 발사가 종료될때까지 대기
                while (laserGo)
                    yield return waitFixedTime;

                // 기믹에 실패하여 폭탄이 파괴되지 않을 경우 폭발
                if (arrBombGo[ranSelect[laserCount]] != null)
                    arrBombGo[ranSelect[laserCount]].GetComponent<Bomb>().Explosion();

                ++laserCount;
                if (laserCount >= 4)
                    break;

                //레이저 기모으는 사운드 재생(루프)
                soundManager.PlayAudio(
                    GetComponent<AudioSource>(), 
                    (int)SoundManager.ESounds.LASERPREPERATIONSOUND,
                    true);
                //Debug.Log("StartLaserCharge");
                // 보스 레이저와 맞게 눈 색 변경
                changeBodyEyeColorCallback?.Invoke(colors[ranSelect[laserCount]]);

                // 레이저 충전 시작
                ChargeGo = ChargeLaser(colors[ranSelect[laserCount]]);
                ChargeGo.transform.parent = laserLaunchTr;
                laserStartTime = Time.time;
                // 보스 회전 시작
                bossRotationCallback?.Invoke(true);
            }

            yield return waitFixedTime;
        }

        changeBodyEyeColorCallback?.Invoke(Color.red);
        bossRotationCallback?.Invoke(false);
        FinishPattern();
    }

    private GameObject LaunchLaser(Quaternion _laserRotation, Color _curColor, int _idx)
    {

        GameObject laserGo = Instantiate(laserPrefab, laserLaunchTr.position, laserLaunchTr.rotation * _laserRotation);
        laserGo.GetComponent<LaserController>().Init(laserDuration, laserLengthPerSec, initWidth, initHeight, _curColor, _idx);

        Destroy(laserGo, laserDuration);
        return laserGo;
    }

    private GameObject ChargeLaser(Color _curColor)
    {
        GameObject laserChargeGo = Instantiate(laserChargePrefab, laserLaunchTr.position+ laserLaunchTr.forward*80f, laserLaunchTr.rotation,transform);
        VisualEffect vfx = laserChargeGo.GetComponent<VisualEffect>();

        vfx.SetFloat("Duration", laserDelay);
        float decreaseChargeDuration = laserDelay - 2f;
        vfx.SetFloat("ChargeDuration", decreaseChargeDuration);
        vfx.SetGradient("Color", SetColorGradient(_curColor));
        vfx.SetGradient("Core Color", SetColorGradient(_curColor * 30f));
        vfx.Reinit();
        Destroy(laserChargeGo, laserDelay); //지속시간 10초 라는 뜻인데 따로 패턴 변수가 없는거같음.
        return laserChargeGo;
    }

    private Gradient SetColorGradient(Color _curColor)
    {
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[4];

        colorKeys[0].color = _curColor;
        colorKeys[0].time = 0f;
        colorKeys[1].color = _curColor;
        colorKeys[1].time = 0f;

        alphaKeys[0].alpha = 0f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 1f;
        alphaKeys[1].time = 0.1f;
        alphaKeys[2].alpha = 0.8f;
        alphaKeys[2].time = 0.8f;
        alphaKeys[3].alpha = 0f;
        alphaKeys[3].time = 1f;

        var gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);
        return gradient;
    }

    private Quaternion CalcLaserRotation()
    {
        float angleToPlayer = Mathf.Asin((targetTr.position.y - laserLaunchTr.position.y) / Vector3.Distance(laserLaunchTr.position, targetTr.position));
        angleToPlayer = Mathf.Clamp(angleToPlayer * Mathf.Rad2Deg, -launchAngleLimit, launchAngleLimit);
        return Quaternion.Euler(Vector3.left * angleToPlayer);
    }

    private void GenerateUniqueRandomNumbers()
    {
        // 가능한 숫자들의 리스트
        List<int> availableNumbers = new List<int>() { 0, 1, 2, 3 }; 

        for (int i = 0; i < 4; i++)
        {
            // 가능한 숫자 중에서 랜덤하게 선택
            int randomIndex = Random.Range(0, availableNumbers.Count);

            // 선택된 숫자를 배열에 추가
            ranSelect[i] = availableNumbers[randomIndex];
            // 사용된 숫자는 가능한 숫자 리스트에서 제거
            availableNumbers.RemoveAt(randomIndex); 
        }
    }


    [Header("-Bomb")]
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
    //[ColorUsage(true,true)]
    private Color[] colors = null;


    [Header("-Laser")]
    [SerializeField]
    private GameObject laserPrefab = null;
    [SerializeField]
    private GameObject laserChargePrefab = null;
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
    [SerializeField]
    private bool isDebugMode = false;

    private GameObject[] arrBombGo = null;
    private VoidVoidDelegate patternFinishCallback = null;
    private VoidBoolDelegate bossRotationCallback = null;
    private VoidVoidDelegate reloadCannonCallback = null;
    private VoidVoidDelegate alertFirstPatternCallback = null;
    private WaitForFixedUpdate waitFixedTime = null;
    private Transform targetTr = null;
    private int[] ranSelect = new int[4];

    private SoundManager soundManager = null;
    private float curLaserLength = 0f;
}
