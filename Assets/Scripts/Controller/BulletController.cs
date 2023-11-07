using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : AttackableObject
{
    Rigidbody rb = null;
    public float speed = 200f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    public void Init(Quaternion _transform)
    {
        transform.rotation = _transform;
    }
    private void Start()
    {
        rb.velocity = transform.forward*speed;
    }

    private void OnTriggerEnter(Collider _other)
    {
        AttackDmg(_other);
        if(!_other.CompareTag("GatlingGunMuzzle"))
            Destroy(gameObject);
    }

}
