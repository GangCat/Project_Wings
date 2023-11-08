using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    public Vector3 targetPosition; // ��ǥ ������ ��ġ
    public float maxHeight = 10f; // �ִ� ��
    public float initialSpeed = 10f; // �ʱ� �ӵ�

    private float gravity = Physics.gravity.y; // �߷� ���ӵ�

    void Update()
    {
        // ������ � ���
        float timeToReachTarget = CalculateTimeToReachTarget();
        if (timeToReachTarget >= 0f)
        {
            float horizontalSpeed = (targetPosition - transform.position).magnitude / timeToReachTarget;
            Vector3 horizontalDirection = (targetPosition - transform.position).normalized;
            Vector3 verticalDirection = Vector3.up;

            // ������ ��� ���ο� ��ġ ���
            float horizontalDistance = horizontalSpeed * Time.deltaTime;
            float verticalDistance = CalculateVerticalDistance(horizontalDistance, horizontalSpeed, timeToReachTarget);
            Vector3 newPosition = transform.position + horizontalDirection * horizontalDistance + verticalDirection * verticalDistance;

            // ���ο� ��ġ�� �̵�
            transform.position = newPosition;
        }
    }

    float CalculateTimeToReachTarget()
    {
        // ��ǥ ���������� �Ÿ�
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // ���� ������ �̵� �ð� ���
        float timeToReachHorizontalTarget = distanceToTarget / initialSpeed;

        // �ִ� ���� �����ϴ� �ð� ���
        float timeToReachMaxHeight = Mathf.Sqrt(2 * maxHeight / -gravity);

        // �� � �ð� ���
        float totalTime = timeToReachMaxHeight + timeToReachHorizontalTarget;

        // ��ǥ ������ ������ �� �ִ� ��� �� � �ð� ��ȯ, �׷��� ������ -1 ��ȯ
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
        // ������ ������� ���� �Ÿ� ���
        float verticalSpeed = maxHeight / Mathf.Sqrt(2 * maxHeight / -gravity); // �ִ� �������� ���� �ӵ�
        float verticalTime = Mathf.Sqrt(2 * maxHeight / -gravity); // �ִ� ���� �����ϴ� �ð�
        float timeRemaining = totalTime - verticalTime; // �ִ� ���� ������ �� ���� �ð�

        // ���� ������ �̵� �Ÿ� ���
        float verticalDistance = verticalSpeed * timeRemaining + 0.5f * gravity * Mathf.Pow(timeRemaining, 2);

        return verticalDistance;
    }
}
