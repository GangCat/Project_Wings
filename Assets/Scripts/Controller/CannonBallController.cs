using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아래로 떨어지는 속도 값을 준다
/// 플레이어와 부딪히면 폭발하고 데미지를 준다.
/// 땅과 부딪히면 폭발한다.
/// </summary>
public class CannonBallController : AttackableObject
{
    private float speed;
    private WaitForFixedUpdate waitFixedUpdate = null;
    private CannonRainMemoryPool memoryPool = null;
    public void Init(float _speed, Vector3 _spawnPos, CannonRainMemoryPool _memoryPool = null)
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
                Destroy(gameObject);
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
