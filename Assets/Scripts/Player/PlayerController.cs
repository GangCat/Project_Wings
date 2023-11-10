using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public void Init(PlayerData _playerData)
    {
        playerData = _playerData;
        moveCtrl = GetComponentInChildren<PlayerMovementController>();
        rotCtrl = GetComponentInChildren<PlayerRotateController>();
        animCtrl = GetComponentInChildren<PlayerAnimationController>();
        colCtrl = GetComponentInChildren<PlayerCollisionController>();
        statHp = GetComponentInChildren<PlayerStatusHp>();
        virtualMouse = GetComponentInChildren<VirtualMouse>();

        playerMesh = GetComponentInChildren<MeshRenderer>();

        playerData.tr = transform;

        moveCtrl.Init(playerData);
        rotCtrl.Init(playerData);
        animCtrl.Init();
        colCtrl.Init(ChangeCollisionCondition, ChangeCollisionCondition, KnockBack);
        statHp.Init(IsDead);
        virtualMouse.Init(playerData);

    }

    private void IsDead()
    {
        Debug.Log("플레이어 사망");
    }

    private void ChangeCollisionCondition(Collision _coli)
    {
        moveCtrl.ChangeCollisionCondition(_coli, true);

        //float knockBackAmount = 0f;

        //GameObject collisionGo = _coli.gameObject;

        //if (collisionGo.CompareTag("Obstacle"))
        //    knockBackAmount = 5f;
        //else if (collisionGo.CompareTag("CannonBall"))
        //    knockBackAmount = 15f;
        //else if (collisionGo.CompareTag("GatlingGunBullet"))
        //    knockBackAmount = 15f;
        //else if (collisionGo.CompareTag("ShakeBodyCollider"))
        //    knockBackAmount = 50f;
        //else if (collisionGo.CompareTag("WindBlow"))
        //    knockBackAmount = 100f;
        //else if (collisionGo.CompareTag("CrossLaser"))
        //    knockBackAmount = 30f;

        //Vector3 knockBackDir = _coli.contacts[0].normal;

        //playerMesh.material.SetFloat("_isDamaged", 1);

        //moveCtrl.KnockBack(knockBackDir.normalized * knockBackAmount);
        //Invoke("ResetPlayerDamagedBollean", 2f);
    }



    private void ChangeCollisionCondition()
    {
        moveCtrl.ChangeCollisionCondition(null, false);
    }

    private void KnockBack(Collider _collider)
    {
        if (gameObject.layer.Equals(LayerMask.NameToLayer("PlayerInvincible")))
            if (_collider.GetComponent<AttackableObject>())
                return;

        float knockBackAmount = 0f;
        Vector3 knockBackDir = (transform.position - _collider.transform.position).normalized;
        float knockBackDelay = 2f;


        if (_collider.CompareTag("CannonBall"))
        {
            knockBackAmount = 50f;
            knockBackDir = Vector3.down;
            Debug.Log("Hit");

        }
        else if (_collider.CompareTag("GatlingGunBullet"))
        {
            knockBackAmount = 50f;
            knockBackDir = _collider.transform.forward;

            Debug.Log("Hit");
        }
        else if (_collider.CompareTag("ShakeBodyCollider"))
        {
            knockBackAmount = 500f;
            knockBackDir = Vector3.up;
            knockBackDelay = 4f;
        }
        else if (_collider.CompareTag("WindBlow"))
        {
            knockBackAmount = 250f;
            knockBackDir = Vector3.up;
            knockBackDelay = 4f;
        }
        else if (_collider.CompareTag("WindBlowForPattern"))
        {
            knockBackAmount = 1000f;
            knockBackDir = Vector3.up;
            knockBackDelay = 3f;
        }
        else if (_collider.CompareTag("CrossLaser"))
        {
            knockBackAmount = 100f;
            knockBackDir = _collider.transform.forward;
            Debug.Log("Hit");
        }
        else if (_collider.CompareTag("BossShield"))
        {
            knockBackAmount = 50f;
        }

        playerMesh.material.SetFloat("_isDamaged", 1);
        StopCoroutine("ResetPlayerDamagedBollean");
        StartCoroutine("ResetPlayerDamagedBollean");
        //Invoke("ResetPlayerDamagedBollean", 2f);

        moveCtrl.KnockBack(knockBackDir.normalized * knockBackAmount, knockBackDelay);
    }

    private IEnumerator ResetPlayerDamagedBollean()
    {
        yield return new WaitForSeconds(2f);
        playerMesh.material.SetFloat("_isDamaged", 0);
    }

    private void Update()
    {
        DebugColorChange();
        moveCtrl.PlayerDodge(playerData.input.InputQ, playerData.input.InputE);
        virtualMouse.UpdateMouseInput();
        animCtrl.SetSpeedFloat(playerData.currentMoveSpeed);
    }

    private void FixedUpdate()
    {
        virtualMouse.FixedUpdateMouseInput();
        rotCtrl.PlayerRotate();
        rotCtrl.PlayerFixedRotate();
        moveCtrl.CalcPlayerMove(playerData.input.InputZ, playerData.input.InputShift);
        moveCtrl.PlayerMove();

        //Debug.Log($"Player Speed: {moveCtrl.MoveSpeed}");

        if (moveCtrl.IsDash)
            screenMat.SetFloat("_isDash", 1);
        else
            screenMat.SetFloat("_isDash", 0);
    }

    private void AnimatorTest()
    {
    }


    private void DebugColorChange()
    {
        if (playerData.isCrash == true)
        {
            playerMesh.material.SetColor("_BaseColor", Color.red);
        }
        else if(gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            playerMesh.material.SetColor("_BaseColor", Color.yellow);
        }
        else if (playerData.isDash == true)
        {
            playerMesh.material.SetColor("_BaseColor", Color.blue);
        }
        else
        {
            playerMesh.material.SetColor("_BaseColor", Color.white);
        }
    }

    private PlayerMovementController moveCtrl = null;
    private PlayerRotateController rotCtrl = null;
    private PlayerAnimationController animCtrl = null;
    private PlayerCollisionController colCtrl = null;
    private PlayerStatusHp statHp = null;

    private MeshRenderer playerMesh = null;

    private VirtualMouse virtualMouse = null;

    private PlayerData playerData = null;

    [SerializeField]
    private Material screenMat = null;
}
