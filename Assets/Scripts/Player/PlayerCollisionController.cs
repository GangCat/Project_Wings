using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public delegate void ChangeCollisionConditionDelegate(Collision _coli, bool _bool);
    public delegate void KnockBackDelegate(Collider _collider, bool _bool);

    private ChangeCollisionConditionDelegate collisionCallback = null;
    private KnockBackDelegate knockBackCallback = null;

    public void Init(ChangeCollisionConditionDelegate _Callback, KnockBackDelegate _knockBackCallback)
    {
        collisionCallback = _Callback;
        knockBackCallback = _knockBackCallback;
        oriLayer = gameObject.layer;
    }


    private void OnCollisionEnter(Collision collision)
    {
        collisionCallback?.Invoke(collision, true);
        Invincible();
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionCallback?.Invoke(collision,false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer.Equals(LayerMask.NameToLayer(playerInvincibleLayer)))
            return;

        knockBackCallback?.Invoke(other, true);

        //if (other.CompareTag("ShakeBodyCollider"))
        //    Debug.Log("�� ���⿡ �ǰݴ���.");
        //else if(other.CompareTag("WindBlow"))
        //    Debug.Log("3���� �ٶ��� �ǰݴ���.");
        //else if (other.CompareTag("CrossLaser"))
        //    Debug.Log("���� �������� �ǰݴ���.");

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
