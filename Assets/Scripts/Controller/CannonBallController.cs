using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ʒ��� �������� �ӵ� ���� �ش�
/// �÷��̾�� �ε����� �����ϰ� �������� �ش�.
/// ���� �ε����� �����Ѵ�.
/// </summary>
public class CannonBallController : MonoBehaviour
{
    private float speed;
    public void Init(float _speed)
    {
        speed = _speed;
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;

            yield return null;
        }
    }
}
