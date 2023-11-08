using System.Collections;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{
    public void Init(Vector3 _targetPos, float _maxHeight, float _initialSpeed)
    {
        targetPosition = _targetPos;
        maxHeight = _maxHeight;
        initialSpeed = _initialSpeed;
        waitFixedTime = new WaitForFixedUpdate();

        initialPosition = transform.position;
        initialTime = Time.time;

        transform.rotation = Quaternion.LookRotation(targetPosition);

        //StartCoroutine("UpdateCoroutine");
    }

    private void Update()
    {
        
        // 포물선 운동 계산
        float horizontalDistance = (targetPosition - initialPosition).magnitude;
        float timeToReachTarget = horizontalDistance / initialSpeed;

        // 수평 방향의 속도 계산
        float horizontalSpeed = initialSpeed;

        // 수직 방향의 초기 속도 계산 (수직 방향으로 동일한 시간에 높이 maxHeight에 도달해야 함)
        float verticalSpeed = (targetPosition.y - initialPosition.y) / timeToReachTarget - 0.5f * gravity * timeToReachTarget;

        // 이동 전 오브젝트의 진행방향을 향해 회전
        //Vector3 targetDirection = (targetPosition - transform.position).normalized;
        //transform.forward = targetDirection;

        // 포물선 운동의 새로운 위치 계산
        float currentTime = Time.time - initialTime;
        Vector3 newPosition = initialPosition + horizontalSpeed * transform.forward * currentTime +
                              verticalSpeed * Vector3.up * currentTime +
                              0.5f * gravity * Vector3.up * Mathf.Pow(currentTime, 2);

        // 새로운 위치로 이동
        transform.position = newPosition;

        // 목표 지점에 도달하면 운동 중지
        if (currentTime >= timeToReachTarget)
        {
            enabled = false;
        }
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            // 포물선 운동 계산
            float timeToReachTarget = CalculateTimeToReachTarget();
            if (timeToReachTarget >= 0f)
            {
                float horizontalSpeed = (targetPosition - transform.position).magnitude / timeToReachTarget;
                Vector3 horizontalDirection = (targetPosition - transform.position).normalized;
                Vector3 verticalDirection = Vector3.up;

                // 포물선 운동의 새로운 위치 계산
                float horizontalDistance = horizontalSpeed * Time.fixedDeltaTime;
                float verticalDistance = CalculateVerticalDistance(horizontalDistance, horizontalSpeed, timeToReachTarget);
                Vector3 newPosition = transform.position + horizontalDirection * horizontalDistance + verticalDirection * verticalDistance;

                // 새로운 위치로 이동
                transform.position = newPosition;
            }
            yield return waitFixedTime;
        }
        
    }

    float CalculateTimeToReachTarget()
    {
        // 목표 지점까지의 거리
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // 수평 방향의 이동 시간 계산
        float timeToReachHorizontalTarget = distanceToTarget / initialSpeed;

        // 최대 고도에 도달하는 시간 계산
        float timeToReachMaxHeight = Mathf.Sqrt(2 * maxHeight / -gravity);

        // 총 운동 시간 계산
        float totalTime = timeToReachMaxHeight + timeToReachHorizontalTarget;

        // 목표 지점에 도달할 수 있는 경우 총 운동 시간 반환, 그렇지 않으면 -1 반환
        if (totalTime > 0f)
        {
            return totalTime;
        }
        else
        {
            return -1f;
        }
    }

    float CalculateVerticalDistance(float horizontalDistance, float horizontalSpeed, float totalTime)
    {
        // 포물선 운동에서의 수직 거리 계산
        float verticalSpeed = maxHeight / Mathf.Sqrt(2 * maxHeight / -gravity); // 최대 고도에서의 수직 속도
        float verticalTime = Mathf.Sqrt(2 * maxHeight / -gravity); // 최대 고도에 도달하는 시간
        float timeRemaining = totalTime - verticalTime; // 최대 고도에 도달한 후 남은 시간

        // 수직 방향의 이동 거리 계산
        float verticalDistance = verticalSpeed * timeRemaining + 0.5f * gravity * Mathf.Pow(timeRemaining, 2);

        return verticalDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerTimer");

        if(other.CompareTag("Obstacle"))
        {
            StopCoroutine("UpdateCoroutine");
            StartCoroutine("TimerCoroutine");
        }
    }

    private IEnumerator TimerCoroutine()
    {
        float startTime = Time.time;
        while(Time.time - startTime < timer)
        {
            yield return waitFixedTime;
        }

        Explosion();
    }

    private void Explosion()
    {
        // 폭발하며 플레이어에게 큰 데미지
        // 화면 연출
        Destroy(gameObject);
    }


    private Vector3 targetPosition; // 목표 지점의 위치
    private float maxHeight = 10f; // 최대 고도
    private float initialSpeed = 10f; // 초기 속도
    private float gravity = Physics.gravity.y; // 중력 가속도

    private float timer = 0f; // 터질때까지 걸리는 시간

    private WaitForFixedUpdate waitFixedTime = null;

    private Vector3 initialPosition = Vector3.zero;
    float initialTime = 0f;
}
