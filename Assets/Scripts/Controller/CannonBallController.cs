using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아래로 떨어지는 속도 값을 준다
/// 플레이어와 부딪히면 폭발하고 데미지를 준다.
/// 땅과 부딪히면 폭발한다.
/// </summary>
public class CannonBallController : AttackableObject, ISubscriber
{
    private float speed;
    private WaitForFixedUpdate waitFixedUpdate = null;
    private CannonMemoryPool memoryPool = null;
    private bool isPhaseChanged = false;
    public void Init(float _speed, Vector3 _spawnPos, CannonMemoryPool _memoryPool = null)
    {
        speed = _speed;
        transform.position = _spawnPos;
        memoryPool = _memoryPool;
        waitFixedUpdate = new WaitForFixedUpdate();
        isPhaseChanged = false;

        Subscribe();
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

            if(isPhaseChanged)
            {
                memoryPool.DeactivateCannonBall(gameObject);
                yield break;
            }
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (AttackDmg(_other))
            memoryPool.DeactivateCannonBall(gameObject);
    }

    private void OnDisable()
    {
        Broker.UnSubscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void ReceiveMessage(EMessageType _message)
    {
        if (_message == EMessageType.PHASE_CHANGE)
            isPhaseChanged = true;
    }
}
