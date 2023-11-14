using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public delegate void ChangeCollisionConditionDelegate(Collision _coli);
    public delegate void KnockBackDelegate(GameObject _colliderGo);

    private ChangeCollisionConditionDelegate collisionEnterCallback = null;
    private VoidVoidDelegate collisionExitCallback = null;
    private KnockBackDelegate knockBackCallback = null;


    public void Init(ChangeCollisionConditionDelegate _collisionEnterCallback, VoidVoidDelegate _collisionExitCallback, KnockBackDelegate _knockBackCallback, PlayerData _playerData)
    {
        collisionEnterCallback = _collisionEnterCallback;
        collisionExitCallback = _collisionExitCallback;
        knockBackCallback = _knockBackCallback;
        playerData = _playerData;
        oriLayer = gameObject.layer;
        waitInvincibleTime = new WaitForSeconds(invincibleTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        collisionEnterCallback?.Invoke(collision);
        //if(playerData.currentMoveSpeed > 150f)
        //    knockBackCallback?.Invoke(collision.gameObject);

        //Invincible();
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionExitCallback?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        knockBackCallback?.Invoke(other.gameObject);

        Invincible();
    }

    private void Invincible()
    {
        gameObject.layer = LayerMask.NameToLayer(playerInvincibleLayer);
        StopCoroutine("FinishInvincible");
        StartCoroutine("FinishInvincible");
        //Invoke("FinishInvincible", invincibleTime);
    }

    private IEnumerator FinishInvincible()
    {
        yield return waitInvincibleTime;

        gameObject.layer = oriLayer;
    }

    [SerializeField]
    private string playerInvincibleLayer;
    [SerializeField]
    private float invincibleTime = 0f;

    private LayerMask oriLayer;
    private WaitForSeconds waitInvincibleTime = null;
    private PlayerData playerData = null;
}
