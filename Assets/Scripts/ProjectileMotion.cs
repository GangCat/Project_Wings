using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    public Vector3 targetPosition; // 목표 지점의 위치
    public float maxHeight = 10f; // 최대 고도
    public float initialSpeed = 10f; // 초기 속도

    private float gravity = Physics.gravity.y; // 중력 가속도

    void Update()
    {
        // 포물선 운동 계산
        float timeToReachTarget = CalculateTimeToReachTarget();
        if (timeToReachTarget >= 0f)
        {
            float horizontalSpeed = (targetPosition - transform.position).magnitude / timeToReachTarget;
            Vector3 horizontalDirection = (targetPosition - transform.position).normalized;
            Vector3 verticalDirection = Vector3.up;

            // 포물선 운동의 새로운 위치 계산
            float horizontalDistance = horizontalSpeed * Time.deltaTime;
            float verticalDistance = CalculateVerticalDistance(horizontalDistance, horizontalSpeed, timeToReachTarget);
            Vector3 newPosition = transform.position + horizontalDirection * horizontalDistance + verticalDirection * verticalDistance;

            // 새로운 위치로 이동
            transform.position = newPosition;
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
}
