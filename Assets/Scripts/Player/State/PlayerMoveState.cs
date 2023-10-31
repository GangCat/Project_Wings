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

        //// playerData.rotAccle * -playerData.inputHandler.GetInputX() * Time.deltaTime; �� ���ӵ�

        //targetAngle = -playerData.inputHandler.GetInputX() * maxAngle;

        // ������ �־����.
        // ���� ���� ��ġ�ؾ��� ��ġ�� ������ �־����.
        // �ƿ� ��׸��� ó������ ����
        // �׷��� �ִ�, �ּ� ��׸��� �ʿ���.
        // �׸��� ��׸� ��� ���ӵ�, �����ӵ��� �ʿ���.

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
        // a�� ������ -playerData.inputHandler.GetInputX() * maxAngle(90)�� ��������.
        // �׷��� ���� ��ġ�� ���������� �Ÿ��� ���´�.diffAngle
        // �� �� �ִ� �Ÿ� 180�� 1�� ������ ��
        // ���������� �Ÿ��� 180���� ������ ������ ������
        // �� �ð��� ���� ���������� ���� ���������� �ɸ��� �ð� ttlTime�̸�
        // �� �ð����� �����ϴ� �Ÿ��� diffAngle�̹Ƿ�
        // ���ӵ��� ���ϸ� diffAngle / ttlTime�̴�.
        // �� �� �� ���ӵ��� ���غ���
        // �� ���ӵ��� ��������.

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
