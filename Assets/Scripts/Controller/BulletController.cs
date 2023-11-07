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
    public void Init(Quaternion _rotation)
    {
        transform.rotation = _rotation;
    }
    //private void Start()
    //{
    //    rb.velocity = transform.forward * speed;
    //}

    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("GatlingGunMuzzle"))
            return;
        else if (_other.CompareTag("BossShield"))
            return;

        
        if (AttackDmg(_other))
        {
            Destroy(gameObject);
        }
    }

}
