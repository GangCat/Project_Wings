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
        StartCoroutine(ChangeFOV());
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

        resultForwardVelocityLimit = (moveForwardVelocityLimit + moveDashSpeed + gravitySpeed) * dodgeSpeedRatio;
        currentForwardVelocityLimit = Mathf.Lerp(currentForwardVelocityLimit, resultForwardVelocityLimit, 0.7f * Time.deltaTime);
        moveSpeed = Mathf.Clamp(moveSpeed, moveBackVelocityLimit, currentForwardVelocityLimit);

        if (!isKnockBack)
            playerVelocity = moveSpeed * playerTr.forward;


        if (isCollision)
        {
            float angle = Vector3.Angle(playerVelocity, coli.contacts[0].normal);
            
            if (angle >= 120)
            {
                CollisionCrash();
            }

            calcMoveSpeed = currentForwardVelocityLimit * Mathf.Clamp01((1 - angle / 170));
            moveSpeed = Mathf.Lerp(moveSpeed, calcMoveSpeed, moveAccel * Time.deltaTime);
            //Debug.Log(moveSpeed);

            playerVelocity = moveSpeed * playerTr.forward;

            if (CheckisSliding())
            {
                Vector3 normal = coli.contacts[0].normal;
                playerVelocity = playerVelocity - Vector3.Project(playerVelocity, normal);
            }
            else
                isCollision = false;
        }

    }

    public void KnockBack(Vector3 _knockBackAmount)
    {
        StartCoroutine(KnockBackCoroutine(_knockBackAmount));
    }

    private IEnumerator KnockBackCoroutine(Vector3 _knockBackAmount)
    {
        moveSpeed = 0f;
        isKnockBack = true;
        float elapsedTime = 0f;
        while (elapsedTime < knockBackDelay)
        {
            if(!isCollision)
                playerVelocity = Vector3.Slerp(_knockBackAmount, Vector3.zero, elapsedTime / knockBackDelay);

            elapsedTime += Time.deltaTime;

            yield return waitFixedUpdate;
        }
        playerVelocity = Vector3.zero;
        isKnockBack = false;
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
        Debug.Log(isCollision);
        playerData.currentMoveSpeed = moveSpeed; // 현재 속도 공유
    }


    public void PlayerDodge(bool _inputQ, bool _inputE)
    {
        if (_inputQ == true && isDodge == false)
        {
            Vector3 forwardLeft = Vector3.Cross(playerTr.forward, Vector3.up);
            isDodge = true;
            StartCoroutine(MoveToDir(playerData.dodgeSpeed, dodgeDuration, forwardLeft));
        }

        if (_inputE == true && isDodge == false)
        {
            Vector3 forwardRight = Vector3.Cross(playerTr.forward, Vector3.down);
            isDodge = true;
            StartCoroutine(MoveToDir(playerData.dodgeSpeed, dodgeDuration, forwardRight));
        }

    }

    private IEnumerator MoveToDir(float speed, float duration, Vector3 _dir)
    {
        Vector3 direction = _dir;
        float distance = speed * duration;
        float movedDistance = 0f;

        while (movedDistance < distance)
        {
            // 이걸 playervelocity에 좌, 혹은 우 방향으로 곱해주면 될듯? 더해주면 될려나
            float step = speed * Time.deltaTime;
            RaycastHit hit;
            if (rb.SweepTest(direction, out hit, step))
            {
                isDodge = false;
                break;
            }
            Vector3 new_pos = rb.position + direction * step;
            rb.MovePosition(new_pos);
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

    private IEnumerator ChangeFOV()
    {
        float fovLerpRate = 0.1f;
        float targetFOV = Camera.main.fieldOfView;

        while (true)
        {
            float speedRatio = Mathf.InverseLerp(cameraMinSpeed, cameraMaxSpeed, moveSpeed); // 카메라의 속도의 비율에 따라 변경됨.
            targetFOV = Mathf.Lerp(cameraminFOV, cameramaxFOV, speedRatio);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, fovLerpRate);
            yield return null;
        }
    }




    private Vector3 playerVelocity = Vector3.zero;


    private Vector3 velocitySmoothDamp = Vector3.zero;

    private WaitForFixedUpdate waitFixedUpdate = null;

    private float calcMoveSpeed = 0f;

    private float moveBackVelocityLimit = 0f;
    private float moveForwardVelocityLimit = 0f;
    private float resultForwardVelocityLimit = 0f;
    private float currentForwardVelocityLimit = 0f;


    private float moveSpeed = 0f;
    private float moveAccel = 0f;

    private float moveDashSpeed = 0f;
    private float moveDashAccel = 0f;

    private float gravityAccel = 0f;
    private float gravitySpeed = 0f;

    private float moveStopAccel = 0f;
    private float moveAccelResult = 0f;

    private float dodgeSpeedRatio = 1f;


    private bool isDash = false;
    private bool isDodge = false;
    private bool isCollision = false;
    private bool isKnockBack = false;


    [SerializeField]
    private Rigidbody rb = null;
    [SerializeField]
    private float dodgeDuration = 1f;
    [SerializeField]
    private float knockBackDelay = 5f;

    private Transform playerTr = null;
    private PlayerData playerData = null;

    private Collision coli = null;


    public float cameraSpeed;
    public float cameraMinSpeed = 60f;
    public float cameraMaxSpeed = 90f;
    public float cameraminFOV = 70f;
    public float cameramaxFOV = 90f;



}
