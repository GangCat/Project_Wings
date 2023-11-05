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
        colCtrl.Init(ChangeCollisionCondition);
        virtualMouse.Init(playerData);

    }

    private void ChangeCollisionCondition(Collision _Coli ,bool _bool)
    {
        moveCtrl.ChangeCollisionCondition(_Coli, _bool);
    }

    private void Update()
    {

        moveCtrl.CalcPlayerMove(playerData.input.InputZ, playerData.input.InputShift);

        if (playerData.isCrash == true)
        {
            playerMesh.material.SetColor("_BaseColor", Color.red);
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
