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
    Rigidbody rb = null;
    public float speed =-150f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    private void Start()
    {
        rb.velocity = new Vector3(0f,speed,0f);
    }

    private void Update()
    {
    }

}
