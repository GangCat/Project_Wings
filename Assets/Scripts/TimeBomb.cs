using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeBomb : MonoBehaviour
{
    public void Init(Vector3 _targetPos, float _launchAngle, float _gravity, float _explosionTime, Transform _targetTr)
    {
        launchAngle = _launchAngle;
        targetPos = _targetPos;
        gravity = _gravity;
        timer = _explosionTime;
        targetTr = _targetTr;
        waitFixedTime = new WaitForFixedUpdate();
        rb = GetComponent<Rigidbody>();

        //Test();
        StartCoroutine(SimulateProjectile());
    }

    IEnumerator SimulateProjectile()
    {
        // 시작 전 잠시 딜레이
        yield return new WaitForSeconds(0.1f);

        // 거리 계산
        float targetDistance = Vector3.Distance(transform.position, targetPos);

        float sinAngle = Mathf.Sin(launchAngle * Mathf.Deg2Rad);
        float cosinAngle = Mathf.Cos(launchAngle * Mathf.Deg2Rad);

        // 각도와 거리를 이용한 초기속도 계산
        float initVelocity = Mathf.Sqrt(targetDistance / (2 * sinAngle * cosinAngle / gravity));

        // 초기 속도를 이용한 수평, 수직 속도 계산
        float HorizontalVelocity = initVelocity * cosinAngle;
        float VerticalVelocity = initVelocity * sinAngle;

        // 총 비행시간 계산
        float flightDuration = targetDistance / HorizontalVelocity;

        // 타겟 방향으로 회전
        // Translate이기 때문에 돌림
        transform.rotation = Quaternion.LookRotation(targetPos - projectile.position);

        float elapsedtime = 0;
        while (elapsedtime < flightDuration)
        {
            transform.Translate(0, (VerticalVelocity - (gravity * elapsedtime)) * Time.fixedDeltaTime, HorizontalVelocity * Time.fixedDeltaTime);
            elapsedtime += Time.fixedDeltaTime;

            yield return waitFixedTime;
        }
    }

    public void StartTimer(float _time)
    {
        StartCoroutine("TimerCoroutine", _time);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trig");

        if (other.CompareTag("BossLaser"))
            Destroy(gameObject);
    }

    private IEnumerator TimerCoroutine(float _time)
    {
        Debug.Log("TriggerTimer");

        yield return new WaitForSeconds(_time);
        //float startTime = Time.time;
        //while (Time.time - startTime < timer)
        //{
        //    yield return waitFixedTime;
        //}
        Explosion();
    }

    private void Explosion()
    {
        // 폭발하며 플레이어에게 큰 데미지
        targetTr.GetComponent<IDamageable>().GetDamage(150);
        // 화면 연출
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.TIMEBOMBTIMEFLOWSOUND);
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.TIMEBOMBEXPLOSIONSOUND);
        Destroy(Instantiate(explosionPrefab, transform.position, Quaternion.identity), 5f);
        Debug.Log("Explosion!");
        Destroy(gameObject);
    }


    private Vector3 targetPos;
    private float launchAngle = 45.0f;
    private float gravity = 9.81f;
    private float timer = 0f; // 터질때까지 걸리는 시간

    [SerializeField]
    private Transform projectile;
    [SerializeField]
    private GameObject explosionPrefab = null;

    private WaitForFixedUpdate waitFixedTime = null;
    private Rigidbody rb = null;
    private Transform targetTr = null;
    private SoundManager soundManager = SoundManager.Instance;
}
