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

        playerData.tr = transform;

        moveCtrl.Init(playerData);
        rotCtrl.Init(playerData);
        animCtrl.Init();
        colCtrl.Init();
    }

    private void Update()
    {
        moveCtrl.PlayerMove(playerData.input.InputZ);
        rotCtrl.PlayerRotate();
    }

    private PlayerMovementController moveCtrl = null;
    private PlayerRotateController rotCtrl = null;
    private PlayerAnimationController animCtrl = null;
    private PlayerCollisionController colCtrl = null;

    private PlayerData playerData = null;
}
