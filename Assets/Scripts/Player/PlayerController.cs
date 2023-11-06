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
        virtualMouse = GetComponentInChildren<VirtualMouse>();

        playerMesh = GetComponentInChildren<MeshRenderer>();

        playerData.tr = transform;

        moveCtrl.Init(playerData);
        rotCtrl.Init(playerData);
        animCtrl.Init();
        colCtrl.Init(ChangeCollisionCondition, ChangeCollisionCondition, KnockBack);
        virtualMouse.Init(playerData);

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

    private void ResetPlayerDamagedBollean()
    {
        playerMesh.material.SetFloat("_isDamaged", 0);
    }

    private void ChangeCollisionCondition()
    {
        moveCtrl.ChangeCollisionCondition(null, false);
    }

    private void KnockBack(Collider _collider)
    {
        float knockBackAmount = 0f;
        Vector3 knockBackDir = (transform.position - _collider.transform.position).normalized;
        float knockBackDelay = 0f;

        if (_collider.CompareTag("CannonBall"))
        {
            knockBackAmount = 15f;
            knockBackDir = Vector3.down;
        }
        else if (_collider.CompareTag("GatlingGunBullet"))
        {
            knockBackAmount = 15f;
            knockBackDir = _collider.transform.forward;
        }
        else if (_collider.CompareTag("ShakeBodyCollider"))
        {
            knockBackAmount = 200f;
            knockBackDir = Vector3.up;
            knockBackDelay = 4f;
        }
        else if (_collider.CompareTag("WindBlow"))
        {
            knockBackAmount = 100f;
            knockBackDelay = 4f;
        }
        else if (_collider.CompareTag("CrossLaser"))
        {
            knockBackAmount = 30f;
            knockBackDir = _collider.transform.forward;
        }

        playerMesh.material.SetFloat("_isDamaged", 1);
        Invoke("ResetPlayerDamagedBollean", 2f);

        moveCtrl.KnockBack(knockBackDir.normalized * knockBackAmount, knockBackDelay);
    }

    private void Update()
    {

        DebugColorChange();

        moveCtrl.CalcPlayerMove(playerData.input.InputZ, playerData.input.InputShift);
        virtualMouse.UpdateMouseInput();
        rotCtrl.PlayerRotate();
    }

    private void FixedUpdate()
    {
        virtualMouse.FixedUpdateMouseInput();
        rotCtrl.PlayerFixedRotate();
        moveCtrl.PlayerMove();
        moveCtrl.PlayerDodge(playerData.input.InputQ, playerData.input.InputE);
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

    private MeshRenderer playerMesh = null;

    private VirtualMouse virtualMouse = null;

    private PlayerData playerData = null;
}
