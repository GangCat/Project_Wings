using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

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
            // �Ͼ�� �ִϸ��̼�
            // �ִϸ��̼� ����Ǹ� Ż��
            yield return waitFixedTime;
            break;
        }

        float spawnTime = Time.time;
        int bombIdx = 0;
        while (bombIdx < 4)
        {
            if (Time.time - spawnTime > spawnBombDelay)
            {
                //������ź �߻� ���� ���
                GameObject bombGo = Instantiate(timeBombPrefab, timeBombSpawnTr.position, Quaternion.identity);
                bombGo.GetComponent<TimeBomb>().Init(timeBombDestTr[bombIdx].position, launchAngle, gravity, targetTr, colors[bombIdx], bombIdx);
                arrBombGo[bombIdx] = bombGo;
                ++bombIdx;
                spawnTime = Time.time;
                reloadCannonCallback?.Invoke();
            }

            yield return waitFixedTime;
        }

        // ���� ��ź�� �� ����������� ��ٸ��� �� �÷��̾� Ȯ���϶�� �ǹ��� ���ð�
        yield return new WaitForSeconds(4f);
        bossRotationCallback?.Invoke(true);

        float laserStartTime = Time.time;
        int laserCount = 0;
        GameObject laserGo = null;
        GameObject ChargeGo = null;
        Debug.Log("StartLaserCharge");
        ChargeGo = ChargeLaser();
        //������ ������� ���� ���(����)

        // 0~3 ���ڸ� �������� �����ؼ� ������ ����� ranSelect�迭�� ���� / 0 - 2 - 3 - 1 �� ���� �����
        GenerateUniqueRandomNumbers();

        while (true)
        {
            if (Time.time - laserStartTime > laserDelay - 1f)
            {
                //������ ������� ���� ����
                bossRotationCallback?.Invoke(false);
                Quaternion laserRotation = CalcLaserRotation();

                while (Time.time - laserStartTime < laserDelay)
                    yield return waitFixedTime;

                laserGo = LaunchLaser(laserRotation, colors[ranSelect[laserCount]], ranSelect[laserCount]);
            }

            if (laserGo)
            {
                while (laserGo)
                    yield return waitFixedTime;

                if (arrBombGo[ranSelect[laserCount]] != null)
                    arrBombGo[ranSelect[laserCount]].GetComponent<TimeBomb>().Explosion();

                ++laserCount;
                if (laserCount >= 4)
                    break;

                //������ ������� ���� ���(����)
                Debug.Log("StartLaserCharge");
                ChargeGo = ChargeLaser();
                laserStartTime = Time.time;
                bossRotationCallback?.Invoke(true);
            }

            yield return waitFixedTime;
        }

        bossRotationCallback?.Invoke(false);
        //bool isPatternFinish = true;
        //while (true)
        //{
        //    isPatternFinish = true;
        //    foreach(GameObject go in arrBombGo)
        //    {
        //        if (go != null)
        //        {
        //            isPatternFinish = false;
        //            break;
        //        }
        //    }

        //    if (isPatternFinish)
        //        break;

        //    yield return waitFixedTime;
        //}

        FinishPattern();
    }

    private GameObject LaunchLaser(Quaternion _laserRotation, Color _curColor, int _idx)
    {

        GameObject laserGo = Instantiate(laserPrefab, laserLaunchTr.position, laserLaunchTr.rotation * _laserRotation);
        laserGo.GetComponent<LaserController>().Init(laserDuration, laserLengthPerSec, initWidth, initHeight, _curColor, _idx);

        Destroy(laserGo, laserDuration);
        return laserGo;
    }

    private GameObject ChargeLaser()
    {
        GameObject laserGo = Instantiate(laserChargePrefab, laserLaunchTr.position+ laserLaunchTr.forward*5f, laserLaunchTr.rotation,transform);
        VisualEffect vfx = laserChargePrefab.GetComponent<VisualEffect>();
        vfx.SetFloat("Duration", laserDelay);
        float decreaseChargeDuration = laserDelay - 2f;
        vfx.SetFloat("ChargeDuration", decreaseChargeDuration);
        vfx.Reinit();
        Destroy(laserGo, laserDelay); //���ӽð� 10�� ��� ���ε� ���� ���� ������ ���°Ű���.
        return laserGo;
    }

    private Quaternion CalcLaserRotation()
    {
        float angleToPlayer = Mathf.Asin((targetTr.position.y - laserLaunchTr.position.y) / Vector3.Distance(laserLaunchTr.position, targetTr.position));
        angleToPlayer = Mathf.Clamp(angleToPlayer * Mathf.Rad2Deg, -launchAngleLimit, launchAngleLimit);
        return Quaternion.Euler(Vector3.left * angleToPlayer);
    }

    private void GenerateUniqueRandomNumbers()
    {
        List<int> availableNumbers = new List<int>() { 0, 1, 2, 3 }; // ������ ���ڵ��� ����Ʈ

        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, availableNumbers.Count); // ������ ���� �߿��� �����ϰ� ����

            ranSelect[i] = availableNumbers[randomIndex]; // ���õ� ���ڸ� �迭�� �߰�
            availableNumbers.RemoveAt(randomIndex); // ���� ���ڴ� ������ ���� ����Ʈ���� ����
        }
    }


    [Header("-TimeBomb")]
    [SerializeField]
    private GameObject timeBombPrefab = null;
    [SerializeField]
    private Transform[] timeBombDestTr = null;
    [SerializeField]
    private Transform timeBombSpawnTr = null;
    [SerializeField]
    private float spawnBombDelay = 0f; // ��ź 4�� ���� �����ϴ� ������
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

    private GameObject[] arrBombGo = null;
    private VoidVoidDelegate patternFinishCallback = null;
    private VoidBoolDelegate bossRotationCallback = null;
    private VoidVoidDelegate reloadCannonCallback = null;
    private WaitForFixedUpdate waitFixedTime = null;
    private Transform targetTr = null;
    private int[] ranSelect = new int[4];

    private float curLaserLength = 0f;
}
