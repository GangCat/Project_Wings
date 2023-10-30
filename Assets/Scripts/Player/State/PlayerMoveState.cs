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
        maxAngle = 90;
    }

    public override void Exit()
    {
    }

    public override void LogicUpdate()
    {
        accleVelocity = tr.forward * playerData.inputHandler.GetInputZ() * playerData.maxSpeed;
        accleTime = playerData.accle * Time.deltaTime;
        rb.velocity = Vector3.MoveTowards(rb.velocity, accleVelocity, accleTime);

        rotateAngle = playerData.inputHandler.GetInputX()* maxAngle/* * playerData.maxRotSpeed  Time.deltaTime*/;
        rotAccleTime = playerData.rotAccle /* Time.deltaTime * Time.deltaTime*/;
        //float tempAngle = playerData.inputHandler.GetInputX();
        currentRotation = tr.rotation;

        //tr.Rotate(new Vector3(0, 0, -rotateVelocity));
        //tr.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.MoveTowards(tr.rotation.z * 90, rotateVelocity, rotAccleTime)));
        //tr.rotation = Quaternion.Euler(new Vector3(0f, 0f, playerData.inputHandler.GetInputX() * maxAngle));
        //tr.rotation = Quaternion.Lerp(currentRotation, Quaternion.Euler(new Vector3(0f,0f,-rotateAngle)), rotAccleTime);
        //tr.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.MoveTowards(tr.rotation.z * 180, playerData.inputHandler.GetInputX() * maxAngle, rotAccleTime)));



        //tr.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.MoveTowardsAngle(tr.rotation.z, playerData.inputHandler.GetInputX(), rotAccleTime) * -90));

        //float currentAngle = tr.rotation.z * Mathf.Rad2Deg;
        //currentAngle = Mathf.SmoothDampAngle(currentAngle, rotateAngle, ref angleVelocity, rotAccleTime * (rotateAngle < 10 ? currentAngle / maxAngle : 1));
        //tr.rotation = Quaternion.Euler(new Vector3(0f, 0f, -currentAngle));


        //tr.Rotate(new Vector3(0, 0, Mathf.MoveTowards(tr.rotation.z, rotateVelocity, rotAccleTime)));
        Debug.Log("Rotate.z " + currentAngle);
        //Debug.Log("TempAngle " + tempAngle);
        //Debug.Log("RotateV " + -rotateVelocity);
        //Debug.Log("RotAcc " + rotAccleTime);


        // 현재 각도를 부드럽게 목표 각도로 이동시킵니다.
        float newAngle = Mathf.MoveTowardsAngle(tr.rotation.eulerAngles.z, -rotateAngle, rotAccleTime * Time.deltaTime);

        // 새로운 각도로 회전시킵니다.
        tr.rotation = Quaternion.Euler(0, 0, newAngle);



        //Quaternion targetRotation = Quaternion.Euler(0, 0, rotateAngle);


//        float smoothFactor = rotAccleTime * Time.deltaTime;
  //      tr.rotation = Quaternion.Lerp(currentRotation, targetRotation, smoothFactor);









    }

    public override void PhysicsUpdate()
    {
    }

    private Vector3 accleVelocity;
    private Rigidbody rb;
    private Transform tr;

    private Quaternion currentRotation;
    private float angleVelocity;
    float currentAngle;

    private float accleTime;

    private float rotateAngle;
    private float rotAccleTime;
    private float maxAngle;

    private float horizontal;
    private float vertical;


}
