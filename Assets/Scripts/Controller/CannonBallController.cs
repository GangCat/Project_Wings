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
