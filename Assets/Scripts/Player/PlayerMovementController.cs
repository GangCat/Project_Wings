using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

public class PlayerMovementController : MonoBehaviour
{
    public void Init(PlayerData _playerData)
    {
        playerData = _playerData;
        playerTr = playerData.tr;
        waitFixedUpdate = new WaitForFixedUpdate();

        moveBackVelocityLimit = playerData.moveBackVelocityLimit;
        moveForwardVelocityLimit = playerData.moveForwardVelocityLimit;
        moveAccel = playerData.moveAccel;

        moveDashAccel = playerData.moveDashAccel;
        moveStopAccel = playerData.moveStopAccel;
        
    }

    public void ChangeCollisionCondition(Collision collision, bool _bool)
    {
        isCollision = _bool;
        coli = collision;
    }


        public void CalcPlayerMove(float _inputZ, bool _inputShift)
    {
       // if (Mathf.Abs(_inputZ) > 0f)
        if (_inputZ != 0f)
            {
            isDash = _inputShift;
            playerData.isDash = isDash;
            moveAccelResult = isDash ? moveAccel + moveDashAccel : moveAccel;
            moveDashSpeed = isDash ? playerData.moveDashSpeed : 0f;

            if (moveSpeed > 20f)
            {
                float forwardY = playerTr.forward.y;
                if (forwardY >= 0.3f)
                {

                }
                else if (forwardY <= -0.2f)
                {
                    gravitySpeed = playerData.gravitySpeed;
                }
                else
                {
                    gravitySpeed = 0;
                }
            }

            moveSpeed += (moveAccelResult + gravityAccel) * Time.deltaTime * _inputZ;
        }
        else if (moveSpeed > 0f && _inputZ < 0f)
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, moveAccel * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, moveStopAccel * Time.deltaTime);
            isDash = false;
        }

        ResultForwardVelocityLimit = (moveForwardVelocityLimit + moveDashSpeed + gravitySpeed) * dodgeSpeedRatio;
        currentForwardVelocityLimit = Mathf.Lerp(currentForwardVelocityLimit, ResultForwardVelocityLimit, 0.7f * Time.deltaTime);

        moveSpeed = Mathf.Clamp(moveSpeed, moveBackVelocityLimit, currentForwardVelocityLimit);
        playerVelocity = moveSpeed * playerTr.forward;

        if (isCollision)
        {
            float angle = Vector3.Angle(playerVelocity, coli.contacts[0].normal);

            calcMoveSpeed = currentForwardVelocityLimit * Mathf.Clamp01((1 - angle / 170));
            moveSpeed = Mathf.Lerp(moveSpeed, calcMoveSpeed, moveAccel * Time.deltaTime);
            //Debug.Log(moveSpeed);
            playerVelocity = moveSpeed * playerTr.forward;
            if (angle >= 120)
            {
                CollisionCrash();
            }

            if (CheckisSliding())
            {
                Vector3 normal = coli.contacts[0].normal;
                playerVelocity = playerVelocity - Vector3.Project(playerVelocity, normal);
            }
            else
                isCollision = false;
        }

    }


    private void CollisionCrash()
    {
        if (moveSpeed >= 50)
        {
            StartCoroutine(PlayerCrash());
            Debug.Log("강한 충돌");
        }
    }



    private bool CheckisSliding()
    {
        Vector3 vector1 = playerTr.forward;
        Vector3 vector2 = coli.contacts[0].normal;

        float dotProduct = Vector3.Dot(vector1, vector2);
        float magnitude1 = vector1.magnitude;
        float magnitude2 = vector2.magnitude;

        float cosAngle = dotProduct / (magnitude1 * magnitude2);
        float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;

        return angle >= 100;
    }


    public void PlayerMove()
    {

        rb.velocity = playerVelocity;
        playerData.currentMoveSpeed = moveSpeed; // 현재 속도 공유

    }


    public void PlayerDodge(bool _inputQ, bool _inputE)
    {
        if(_inputQ == true && isDodge == false)
        {
            Vector3 forwardLeft = Vector3.Cross(playerTr.forward, Vector3.up);
            StartCoroutine(MoveToDir(playerData.dodgeSpeed, dodgeDuration, forwardLeft));
            isDodge = true;
        }

        if (_inputE == true && isDodge == false)
        {
            Vector3 forwardRight = Vector3.Cross(playerTr.forward, Vector3.down);
            StartCoroutine(MoveToDir(playerData.dodgeSpeed, dodgeDuration, forwardRight));
            isDodge = true;
        }

    }



    private IEnumerator MoveToDir(float speed, float duration, Vector3 _dir)
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


    private IEnumerator PlayerCrash()
    {
        playerData.isCrash = true;
        moveSpeed = Mathf.MoveTowards(moveSpeed, 0, moveAccel * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        playerData.isCrash = false;
    }

    Vector3 pushDirection;
    Vector3 playerVelocity = Vector3.zero;



    private Vector3 velocitySmoothDamp = Vector3.zero;

    private WaitForFixedUpdate waitFixedUpdate = null;

    private float moveSpeed = 0f;
    private float calcMoveSpeed = 0f;
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
    private bool isCollision = false;


    private Collision coli = null;

    [SerializeField]
    private Rigidbody rb = null;


}
