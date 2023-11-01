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

    public void PlayerRotate() // Update 돌리는거
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
        if (_angle < -360) _angle += 360; // angle이 -360보다 작으면 360을 더해줌. 결과적으로 -380이 -20과 같게 계산됨
        if (_angle > 360) _angle -= 360;

        return Mathf.Clamp(_angle, _min, _max); // 대소를 비교하여 범위 안의 값일 경우 _angle을 반환, _min 이하일경우 _min을 반환, _max이상일 경우 _max를 반환함.
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

    private float eulerAngleX = 0f; // 사용안하는듯?
    private float eulerAngleY = 0f; // 이것도

    private float rotCamSpeed = 0f;
    private float rotCamXAxisSensitive = 0f;
    private float rotCamYAxisSensitive = 0f;
    private float minAngleX = 0f;
    private float maxAngleX = 0f;
    private Transform playerTr = null;

    private Vector3 rotVec = Vector3.zero;
    private PlayerData playerData = null;

    private Vector2 mousePos;


    //테스트용
    [SerializeField]
    private MouseChecker mouse = null;


}
