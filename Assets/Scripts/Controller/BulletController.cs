using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : AttackableObject
{
    [SerializeField]
    private float speed = 200f;

    Rigidbody rb = null;
    private GatlinMemoryPool gatlinMemoryPool = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Init(float _destroyTime, Vector3 _position, Quaternion _rotation,GatlinMemoryPool _gatlinMemoryPooll = null)
    {
        Destroy(gameObject, _destroyTime);
        gameObject.transform.position = _position;
        gameObject.transform.rotation = _rotation;
        gatlinMemoryPool = _gatlinMemoryPooll;
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
            gatlinMemoryPool.DeactivateBullet(gameObject);
        }
    }

}
