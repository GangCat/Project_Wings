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
    public void Init(float _speed)
    {
        speed = _speed;
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
        Debug.Log(_other.transform.name);
        AttackDmg(_other);
        Debug.Log("Attack!");
        Destroy(gameObject);
    }


}
