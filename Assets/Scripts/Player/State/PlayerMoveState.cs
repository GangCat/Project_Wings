using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerData playerData) : base(playerData)
    {
        rb = playerData.rb;
        tr = playerData.tr;
    }

    public override void Enter()
    {
        angulerVelocity = 0f;
        maxAngle = 45;
        diffAngle = 0f;
        maxVelocityDeg = 10f;
    }

    public override void Exit()
    {
    }

    public override void LogicUpdate()
    {
        //accleVelocity = tr.forward * playerData.inputHandler.GetInputZ() * playerData.maxSpeed;
        //accleTime = playerData.accle * Time.deltaTime;
        //rb.velocity = Vector3.MoveTowards(rb.velocity, accleVelocity, accleTime);




        if (Mathf.Abs(playerData.inputHandler.GetInputZ()) > 0f)
            moveVelocityM += playerData.accle * Time.deltaTime * playerData.inputHandler.GetInputZ();
        else
            moveVelocityM = Mathf.MoveTowards(moveVelocityM, 0, playerData.accle * Time.deltaTime);

        moveVelocityM = Mathf.Clamp(moveVelocityM, -playerData.maxSpeed, playerData.maxSpeed);

        destMovePos += moveVelocityM * Time.deltaTime;
        tr.position = tr.forward * destMovePos;





        //angulerVelocity += playerData.rotAccle * -playerData.inputHandler.GetInputX() * Time.deltaTime;
        //angulerVelocity = Mathf.Clamp(angulerVelocity, -playerData.maxRotSpeed, playerData.maxRotSpeed);
        //angulerVelocity = Mathf.Clamp(angulerVelocity, -maxAngle, maxAngle);
        //angulerVelocity = playerData.maxRotSpeed * -playerData.inputHandler.GetInputX();
        //rb.angularVelocity = Vector3.MoveTowards(rb.angularVelocity, Vector3.forward * angulerVelocity, playerData.rotAccle * Time.deltaTime);


        //Debug.Log(rb.angularVelocity);
        //Debug.Log(-playerData.inputHandler.GetInputX());

        //// playerData.rotAccle * -playerData.inputHandler.GetInputX() * Time.deltaTime; 각 가속도

        //targetAngle = -playerData.inputHandler.GetInputX() * maxAngle;

        // 라디안을 넣어야함.
        // 현재 내가 위치해야할 위치의 각도를 넣어야함.
        // 아예 디그리를 처음부터 넣자
        // 그러면 최대, 최소 디그리가 필요함.
        // 그리고 디그리 기반 각속도, 각가속도가 필요함.

        //velocityDeg = Mathf.MoveTowards(velocityDeg, playerData.rotMaxVelocityDeg, playerData.rotAccleDeg * Time.deltaTime * -playerData.inputHandler.GetInputX());

        //velocityDeg += playerData.rotAccleDeg * Time.deltaTime * -playerData.inputHandler.GetInputX();






        if (Mathf.Abs(playerData.inputHandler.GetInputX()) > 0f)
            velocityDeg += playerData.rotAccleDeg * Time.deltaTime * -playerData.inputHandler.GetInputX();
        else
            velocityDeg = Mathf.MoveTowards(velocityDeg, 0, playerData.rotAccleDeg * Time.deltaTime);

        velocityDeg = Mathf.Clamp(velocityDeg, -playerData.rotMaxVelocityDeg, playerData.rotMaxVelocityDeg);

        destAngleDeg += velocityDeg * Time.deltaTime;
        tr.rotation = Quaternion.Euler(Vector3.forward * destAngleDeg);
        destAngleDeg = Mathf.Clamp(destAngleDeg, -maxAngle, maxAngle);

        if (Mathf.Abs(destAngleDeg).Equals(maxAngle))
            velocityDeg = 0f;







        //tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.Euler(Vector3.forward * angulerVelocityDeg), 0.5f);
        // a를 누르면 -playerData.inputHandler.GetInputX() * maxAngle(90)이 목적지다.
        // 그러면 현재 위치와 목적지간의 거리ㅏ 나온다.diffAngle
        // 그 때 최대 거리 180을 1로 가정할 때
        // 목적지간의 거리를 180으로 나누면 비율이 나오고
        // 그 시간이 현재 목적지에서 최종 목적지까지 걸리는 시간 ttlTime이며
        // 그 시간동안 가야하는 거리가 diffAngle이므로
        // 각속도를 구하면 diffAngle / ttlTime이다.
        // 이 때 각 가속도를 구해보면
        // 각 가속도는 동일하지.

    }

    public override void PhysicsUpdate()
    {
    }

    private Vector3 accleVelocity;
    private Rigidbody rb;
    private Transform tr;
    private float accleTime;

    private float angulerVelocity;
    private float destAngleDeg;
    private float targetAngle;
    private float maxAngle;
    private float diffAngle;
    private float velocityDeg;
    private float maxVelocityDeg;



    private float moveVelocityM;
    private float moveAccleM;
    private float destMovePos;

    private int tempFactor = 1;
}
