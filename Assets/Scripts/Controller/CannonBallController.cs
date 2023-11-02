using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아래로 떨어지는 속도 값을 준다
/// 플레이어와 부딪히면 폭발하고 데미지를 준다.
/// 땅과 부딪히면 폭발한다.
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
