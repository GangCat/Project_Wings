using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovementController : MonoBehaviour
{
    public void Init(PlayerData _playerData)
    {
        playerData = _playerData;
        playerTr = playerData.tr;
    }

    public void PlayerMove(float _inputZ)
    {
        moveBackVelocityLimit = playerData.moveBackVelocityLimit;
        moveForwardVelocityLimit = playerData.moveForwardVelocityLimit;
        moveAccel = playerData.moveAccel;

        if (Mathf.Abs(_inputZ) > 0f)
            moveVelocity += moveAccel * Time.deltaTime * _inputZ;
        else
            moveVelocity = Mathf.MoveTowards(moveVelocity, 0, moveAccel * Time.deltaTime);

        moveVelocity = Mathf.Clamp(moveVelocity, moveBackVelocityLimit, moveForwardVelocityLimit);
        playerTr.Translate(playerTr.forward * moveVelocity * Time.deltaTime, Space.World);

        Debug.Log(playerTr.forward);
        //destMoveZPos += moveVelocity * Time.deltaTime;
        //playerTr.position = playerTr.forward * destMoveZPos;
    }

    private float moveVelocity = 0f;
    private float moveAccel = 0f;
    private float moveBackVelocityLimit = 0f;
    private float moveForwardVelocityLimit = 0f;
    private float destMoveZPos = 0f;
    private Transform playerTr = null;

    private PlayerData playerData = null;
}
