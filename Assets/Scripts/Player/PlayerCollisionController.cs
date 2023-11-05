using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public delegate void ChangeCollisionConditionDelegate(Collision _coli, bool _bool);
    private ChangeCollisionConditionDelegate collisionCallback = null;

    public void Init(ChangeCollisionConditionDelegate _Callback)
    {
        collisionCallback = _Callback;
        oriLayer = gameObject.layer;
    }


    private void OnCollisionEnter(Collision collision)
    {
        collisionCallback?.Invoke(collision, true);
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionCallback?.Invoke(collision,false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer.Equals(LayerMask.NameToLayer(playerInvincibleLayer)))
            return;

        if (other.CompareTag("ShakeBodyCollider"))
        {
            Debug.Log("몸 흔들기에 피격당함.");
            gameObject.layer = LayerMask.NameToLayer(playerInvincibleLayer);
            Invoke("FinishInvincible", invincibleTime);
        }
        else if(other.CompareTag("WindBlow"))
        {
            Debug.Log("3방향 바람에 피격당함.");
            gameObject.layer = LayerMask.NameToLayer(playerInvincibleLayer);
            Invoke("FinishInvincible", invincibleTime);
        }
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
