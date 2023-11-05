using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public delegate void ChangeCollisionConditionDelegate(Collision _coli);
    public delegate void KnockBackDelegate(Collider _collider, bool _bool);

    private ChangeCollisionConditionDelegate collisionEnterCallback = null;
    private VoidVoidDelegate collisionExitCallback = null;
    private KnockBackDelegate knockBackCallback = null;


    public void Init(ChangeCollisionConditionDelegate _collisionEnterCallback, VoidVoidDelegate _collisionExitCallback, KnockBackDelegate _knockBackCallback)
    {
        collisionEnterCallback = _collisionEnterCallback;
        collisionExitCallback = _collisionExitCallback;
        knockBackCallback = _knockBackCallback;
        oriLayer = gameObject.layer;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.layer.Equals(LayerMask.NameToLayer(playerInvincibleLayer)))
            return;

        collisionEnterCallback?.Invoke(collision);
        //Invincible();
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionExitCallback?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer.Equals(LayerMask.NameToLayer(playerInvincibleLayer)))
            return;

        knockBackCallback?.Invoke(other, true);

        Invincible();
    }

    private void Invincible()
    {
        gameObject.layer = LayerMask.NameToLayer(playerInvincibleLayer);
        Invoke("FinishInvincible", invincibleTime);
    }

    private void FinishInvincible()
    {
        gameObject.layer = oriLayer;
    }

    [SerializeField]
    private string playerInvincibleLayer;
    [SerializeField]
    private float invincibleTime = 0f;

    private LayerMask oriLayer;
}
