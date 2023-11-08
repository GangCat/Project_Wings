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
        
        // ������ � ���
        float horizontalDistance = (targetPosition - initialPosition).magnitude;
        float timeToReachTarget = horizontalDistance / initialSpeed;

        // ���� ������ �ӵ� ���
        float horizontalSpeed = initialSpeed;

        // ���� ������ �ʱ� �ӵ� ��� (���� �������� ������ �ð��� ���� maxHeight�� �����ؾ� ��)
        float verticalSpeed = (targetPosition.y - initialPosition.y) / timeToReachTarget - 0.5f * gravity * timeToReachTarget;

        // �̵� �� ������Ʈ�� ��������� ���� ȸ��
        //Vector3 targetDirection = (targetPosition - transform.position).normalized;
        //transform.forward = targetDirection;

        // ������ ��� ���ο� ��ġ ���
        float currentTime = Time.time - initialTime;
        Vector3 newPosition = initialPosition + horizontalSpeed * transform.forward * currentTime +
                              verticalSpeed * Vector3.up * currentTime +
                              0.5f * gravity * Vector3.up * Mathf.Pow(currentTime, 2);

        // ���ο� ��ġ�� �̵�
        transform.position = newPosition;

        // ��ǥ ������ �����ϸ� � ����
        if (currentTime >= timeToReachTarget)
        {
            enabled = false;
        }
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            // ������ � ���
            float timeToReachTarget = CalculateTimeToReachTarget();
            if (timeToReachTarget >= 0f)
            {
                float horizontalSpeed = (targetPosition - transform.position).magnitude / timeToReachTarget;
                Vector3 horizontalDirection = (targetPosition - transform.position).normalized;
                Vector3 verticalDirection = Vector3.up;

                // ������ ��� ���ο� ��ġ ���
                float horizontalDistance = horizontalSpeed * Time.fixedDeltaTime;
                float verticalDistance = CalculateVerticalDistance(horizontalDistance, horizontalSpeed, timeToReachTarget);
                Vector3 newPosition = transform.position + horizontalDirection * horizontalDistance + verticalDirection * verticalDistance;

                // ���ο� ��ġ�� �̵�
                transform.position = newPosition;
            }
            yield return waitFixedTime;
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
        // �����ϸ� �÷��̾�� ū ������
        // ȭ�� ����
        Destroy(gameObject);
    }


    private Vector3 targetPosition; // ��ǥ ������ ��ġ
    private float maxHeight = 10f; // �ִ� ��
    private float initialSpeed = 10f; // �ʱ� �ӵ�
    private float gravity = Physics.gravity.y; // �߷� ���ӵ�

    private float timer = 0f; // ���������� �ɸ��� �ð�

    private WaitForFixedUpdate waitFixedTime = null;

    private Vector3 initialPosition = Vector3.zero;
    float initialTime = 0f;
}
