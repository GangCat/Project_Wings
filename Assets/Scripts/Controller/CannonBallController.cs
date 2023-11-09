using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ʒ��� �������� �ӵ� ���� �ش�
/// �÷��̾�� �ε����� �����ϰ� �������� �ش�.
/// ���� �ε����� �����Ѵ�.
/// </summary>
public class CannonBallController : AttackableObject
{
    private float speed;
    private WaitForFixedUpdate waitFixedUpdate = null;
    private CannonMemoryPool memoryPool = null;
    public void Init(float _speed, Vector3 _spawnPos, CannonMemoryPool _memoryPool = null)
    {
        speed = _speed;
        transform.position = _spawnPos;
        memoryPool = _memoryPool;
        waitFixedUpdate = new WaitForFixedUpdate();
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if(transform.position.y < 0)
            {
                memoryPool.DeactivateCannonBall(gameObject);
                yield break;
            }

            transform.position += Vector3.down * speed * Time.deltaTime;

            yield return waitFixedUpdate;
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (AttackDmg(_other))
            memoryPool.DeactivateCannonBall(gameObject);
    }
}
