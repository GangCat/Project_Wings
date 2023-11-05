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
        colCtrl.Init(ChangeCollisionCondition, KnockBack);
        virtualMouse.Init(playerData);

    }

    private void ChangeCollisionCondition(Collision _coli ,bool _bool)
    {
        moveCtrl.ChangeCollisionCondition(_coli, _bool);
    }

    private void KnockBack(Collider _collider, bool _bool)
    {
        float knockBackAmount = 0f;

        if (_collider.CompareTag("CannonBall"))
            knockBackAmount = 15f;
        else if (_collider.CompareTag("GatlingGunBullet"))
            knockBackAmount = 15f;
        else if (_collider.CompareTag("ShakeBodyCollider"))
            knockBackAmount = 50f;
        else if (_collider.CompareTag("WindBlow"))
            knockBackAmount = 100f;
        else if (_collider.CompareTag("CrossLaser"))
            knockBackAmount = 30f;

        Vector3 knockBackDir = _collider.transform.forward;

        moveCtrl.KnockBack(knockBackDir.normalized * knockBackAmount);
    }

    private void Update()
    {

        moveCtrl.CalcPlayerMove(playerData.input.InputZ, playerData.input.InputShift);

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


        virtualMouse.UpdateMouseInput();
    }

    private void FixedUpdate()
    {
        virtualMouse.FixedUpdateMouseInput();
        rotCtrl.PlayerRotate();
        moveCtrl.PlayerMove();
        moveCtrl.PlayerDodge(playerData.input.InputQ, playerData.input.InputE);
    }


    private PlayerMovementController moveCtrl = null;
    private PlayerRotateController rotCtrl = null;
    private PlayerAnimationController animCtrl = null;
    private PlayerCollisionController colCtrl = null;

    private MeshRenderer playerMesh = null;

    private VirtualMouse virtualMouse = null;

    private PlayerData playerData = null;
}
