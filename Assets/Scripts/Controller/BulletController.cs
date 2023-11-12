using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : AttackableObject, ISubscriber
{
    [SerializeField]
    private float speed = 200f;

    private GatlinMemoryPool gatlinMemoryPool = null;
    private bool isPhaseChange = false;

    public void Init(float _destroyTime, Vector3 _position, Quaternion _rotation,GatlinMemoryPool _gatlinMemoryPooll = null)
    {
        //Destroy(gameObject, _destroyTime);
        Invoke("DeactivateBullet", _destroyTime);
        gameObject.transform.position = _position;
        gameObject.transform.rotation = _rotation;
        gatlinMemoryPool = _gatlinMemoryPooll;
        isPhaseChange = false;

    }
    //private void Start()
    //{
    //    rb.velocity = transform.forward * speed;
    //}

    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;

        if (isPhaseChange)
            DeactivateBullet();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("GatlingGunMuzzle"))
            return;
        else if (_other.CompareTag("BossShield"))
            return;
        
        
        if (_other.gameObject.layer == LayerMask.NameToLayer("BossBody"))
            DeactivateBullet();
        else if (AttackDmg(_other))
            DeactivateBullet();
    }

    private void DeactivateBullet()
    {
        gatlinMemoryPool.DeactivateBullet(gameObject);
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
            isPhaseChange = true;
    }
}
