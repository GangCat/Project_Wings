using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovementController : MonoBehaviour
{
    public void Init(PlayerData _playerData)
    {
        playerData = _playerData;
        playerTr = playerData.tr;
        waitFixedUpdate = new WaitForFixedUpdate();
    }

    public void PlayerMove(float _inputZ, bool _inputShift)
    {
        moveBackVelocityLimit = playerData.moveBackVelocityLimit;
        moveForwardVelocityLimit = playerData.moveForwardVelocityLimit;
        moveAccel = playerData.moveAccel;
        
        moveDashAccel = playerData.moveDashAccel;
        moveStopAccel = playerData.moveStopAccel;

        if (Mathf.Abs(_inputZ) > 0f) 
        {
            isDash = _inputShift;
            moveAccelResult = isDash ? moveAccel + moveDashAccel : moveAccel;
            moveDashSpeed = isDash ? playerData.moveDashSpeed : 0f;

            if(moveVelocity > 20f)
            {
                if(playerTr.forward.y >= 0.3f)
                {
                    //gravityAccel = -playerData.gravityAccel;
                    //gravitySpeed = -playerData.gravitySpeed;
                }
                else if(playerTr.forward.y <= -0.2f)
                {
                    //gravityAccel = playerData.gravityAccel;
                    gravitySpeed = playerData.gravitySpeed;
                }
                else {
                    //gravityAccel = 0;
                    gravitySpeed = 0;
                }
            }

            moveVelocity += (moveAccelResult + gravityAccel) * Time.deltaTime * _inputZ;
        }
        else if (moveVelocity > 0f && _inputZ < 0f)
        {
            moveVelocity = Mathf.MoveTowards(moveVelocity, 0, moveAccel * Time.deltaTime);
        } 
        else
        {
            moveVelocity = Mathf.MoveTowards(moveVelocity, 0, moveStopAccel * Time.deltaTime);
            isDash = false;
        }

        ResultForwardVelocityLimit = (moveForwardVelocityLimit + moveDashSpeed + gravitySpeed) * dodgeSpeedRatio;
        currentForwardVelocityLimit = Mathf.Lerp(currentForwardVelocityLimit, ResultForwardVelocityLimit, 0.7f * Time.deltaTime);

        moveVelocity = Mathf.Clamp(moveVelocity, moveBackVelocityLimit, currentForwardVelocityLimit);


        //rb.velocity = moveVelocity * playerTr.forward;
        Vector3 targetVelocity = moveVelocity * playerTr.forward;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocitySmoothDamp, 0.1f);
        playerData.currentMoveVelocity = moveVelocity; // 현재 속도 공유

        //Debug.Log(rb.velocity.magnitude);
        //rb.MovePosition(playerTr.forward * moveVelocity * Time.deltaTime);
        //playerTr.Translate(playerTr.forward * moveVelocity * Time.deltaTime, Space.World);
    }

        // 이동 백업용
        //if (Mathf.Abs(_inputZ) > 0f)
        //    moveVelocity += moveAccel * Time.deltaTime * _inputZ;
        //else
        //    moveVelocity = Mathf.MoveTowards(moveVelocity, 0, moveAccel * Time.deltaTime);


    public void PlayerDodge(bool _inputQ, bool _inputE)
    {
        if(_inputQ == true && isDodge == false)
        {
            Vector3 forwardLeft = Vector3.Cross(playerTr.forward, Vector3.up);
            StartCoroutine(MoveToDir(60, 0.3f, forwardLeft));
            isDodge = true;
        }

        if (_inputE == true && isDodge == false)
        {
            Vector3 forwardRight = Vector3.Cross(playerTr.forward, Vector3.down);
            StartCoroutine(MoveToDir(60, 0.3f, forwardRight));
            isDodge = true;
        }

    }

    IEnumerator MoveToDir(float speed, float duration, Vector3 _dir)
    {
        Vector3 direction = _dir; 
        float distance = speed * duration; 
        float movedDistance = 0f; 

        while (movedDistance < distance)
        {
            float step = speed * Time.deltaTime; 
            playerTr.Translate(direction * step, Space.World); 
            movedDistance += step;
            yield return waitFixedUpdate;
        }
        isDodge = false;
    }


    private Vector3 velocitySmoothDamp = Vector3.zero;


    private WaitForFixedUpdate waitFixedUpdate = null;

    private float moveVelocity = 0f;
    private float moveAccel = 0f;
    private float moveBackVelocityLimit = 0f;
    private float moveForwardVelocityLimit = 0f;
    private float ResultForwardVelocityLimit = 0f;


    private Transform playerTr = null;
    private PlayerData playerData = null;



    /// 새로 추가된것
   
    private float moveDashSpeed = 0f;
    private float moveDashAccel = 0f;
    private float moveStopAccel = 0f;
    private float moveAccelResult = 0f;
    private float gravityAccel = 0f;
    private float gravitySpeed = 0f;
    private float dodgeSpeedRatio = 1f;
    private float currentForwardVelocityLimit = 0f;


    private bool isDash = false;
    private bool isDodge = false;


    [SerializeField]
    private Rigidbody rb = null;


}
