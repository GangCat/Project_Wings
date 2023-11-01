using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerRotateController : MonoBehaviour
{
    public void Init(PlayerData _playerData)
    {
        playerData = _playerData;
        rotCamSpeed = playerData.rotCamSpeed;
        rotCamXAxisSensitive = playerData.rotCamXAxisSensitive;
        rotCamYAxisSensitive = playerData.rotCamYAxisSensitive;
        minAngleX = playerData.minAngleX;
        maxAngleX = playerData.maxAngleX;
        playerTr = playerData.tr;

    }

    public void PlayerRotate() // Update �����°�
    {
        RotateToMouse(ref rotVec.x, ref rotVec.y);
        RotateToKeyboard(ref rotVec.z);
        playerTr.rotation = Quaternion.Euler(rotVec);
    }

    private void RotateToMouse(ref float _eulerAngleX, ref float _eulerAngleY)
    {
        mousePos = playerData.mousePos;

        _eulerAngleY += rotCamSpeed * rotCamYAxisSensitive * Time.deltaTime * (mousePos.x / 100);
        _eulerAngleX -= rotCamSpeed * rotCamXAxisSensitive * Time.deltaTime * (mousePos.y / 100);
        _eulerAngleX = ClampAngle(_eulerAngleX, minAngleX, maxAngleX);
        //Debug.Log(_eulerAngleY);
    }

    private float ClampAngle(float _angle, float _min, float _max)
    {
        if (_angle < -360) _angle += 360; // angle�� -360���� ������ 360�� ������. ��������� -380�� -20�� ���� ����
        if (_angle > 360) _angle -= 360;

        return Mathf.Clamp(_angle, _min, _max); // ��Ҹ� ���Ͽ� ���� ���� ���� ��� _angle�� ��ȯ, _min �����ϰ�� _min�� ��ȯ, _max�̻��� ��� _max�� ��ȯ��.
    }

    private void RotateToKeyboard(ref float _eulerAngleZ)
    {
        rollAccel = playerData.rollAccel;
        rollMaxVelocity = playerData.rollMaxVelocity;
        rollMaxAngle = playerData.rollMaxAngle;

        if (Mathf.Abs(playerData.input.InputX) > 0f)
            rollVelocity += rollAccel * Time.deltaTime * -playerData.input.InputX;
        else
            rollVelocity = Mathf.MoveTowards(rollVelocity, 0, rollAccel * Time.deltaTime);

        rollVelocity = Mathf.Clamp(rollVelocity, -rollMaxVelocity, rollMaxVelocity);

        _eulerAngleZ += rollVelocity * Time.deltaTime;
        _eulerAngleZ = Mathf.Clamp(_eulerAngleZ, -rollMaxAngle, rollMaxAngle);

        if (Mathf.Abs(_eulerAngleZ).Equals(rollMaxAngle))
            rollVelocity = 0f;
    }


    /// <summary>
    /// 
    /// </summary>
        //inputMouseX = Input.GetAxis("Mouse X");
        //inputMouseY = Input.GetAxis("Mouse Y");
        //Mathf.Clamp(inputMouseX, -3, 3);
        //Mathf.Clamp(inputMouseY, -3, 3);

        //inputPosX += inputMouseX * sensitive;
        //inputPosY += inputMouseY * sensitive;


        //float radius = 100f;

        //float distanceFromOrigin = Mathf.Sqrt(inputPosX * inputPosX + inputPosY * inputPosY);

        //if (distanceFromOrigin > radius)
        //{
        //    float angle = Mathf.Atan2(inputPosY, inputPosX);
        //    inputPosX = radius * Mathf.Cos(angle);
        //    inputPosY = radius * Mathf.Sin(angle);
        //}

        //Vector2 inputPos = new Vector2(inputPosX, inputPosY);

        //float maxRadius = 90f;

        //if (inputPos.magnitude < maxRadius)
        //{
        //    inputPosX = Mathf.MoveTowards(inputPosX, 0, decreaseSpeed * Time.deltaTime);
        //    inputPosY = Mathf.MoveTowards(inputPosY, 0, decreaseSpeed * Time.deltaTime);
        //}

        //virtualCursor.transform.position = new Vector3(centerPosition.x + inputPosX, centerPosition.y + inputPosY);

        //Debug.Log("input X: " + inputPosX + ", input Y: " + inputPosY);



    private float rollVelocity = 0f;
    private float rollAccel = 0f;
    private float rollMaxVelocity = 0f;
    private float rollMaxAngle = 0f;

    private float eulerAngleX = 0f; // �����ϴµ�?
    private float eulerAngleY = 0f; // �̰͵�

    private float rotCamSpeed = 0f;
    private float rotCamXAxisSensitive = 0f;
    private float rotCamYAxisSensitive = 0f;
    private float minAngleX = 0f;
    private float maxAngleX = 0f;
    private Transform playerTr = null;

    private Vector3 rotVec = Vector3.zero;
    private PlayerData playerData = null;

    private Vector2 mousePos;


    //�׽�Ʈ��
    [SerializeField]
    private MouseChecker mouse = null;


}
