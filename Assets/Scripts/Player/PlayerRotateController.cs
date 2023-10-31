using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerRotateController : MonoBehaviour
{
    public void Init(PlayerData _playerData)
    {
        playerData = _playerData;
        rotCamXAxisSpeed = playerData.rotCamXAxisSpeed;
        rotCamYAxisSpeed = playerData.rotCamYAxisSpeed;
        minAngleX = playerData.minAngleX;
        maxAngleX = playerData.maxAngleX;
        playerTr = playerData.tr;
        rollAccel = playerData.rollAccel;
        rollMaxVelocity = playerData.rollMaxVelocity;
        rollMaxAngle = playerData.rollMaxAngle;
    }

    public void PlayerRotate()
    {
        RotateToMouse(ref rotVec.x, ref rotVec.y);
        RotateToKeyboard(ref rotVec.z);
        playerTr.rotation = Quaternion.Euler(rotVec);

        //playerTr.localRotation *= Quaternion.Euler(Vector3.up);
    }

    private void RotateToMouse(ref float _eulerAngleX, ref float _eulerAngleY)
    {
        _eulerAngleY += playerData.input.InputMouseX * rotCamYAxisSpeed;
        _eulerAngleX -= playerData.input.InputMouseY * rotCamXAxisSpeed;
        _eulerAngleX = ClampAngle(_eulerAngleX, minAngleX, maxAngleX);
    }

    private float ClampAngle(float _angle, float _min, float _max)
    {
        if (_angle < -360) _angle += 360; // angle이 -360보다 작으면 360을 더해줌. 결과적으로 -380이 -20과 같게 계산됨
        if (_angle > 360) _angle -= 360;

        return Mathf.Clamp(_angle, _min, _max); // 대소를 비교하여 범위 안의 값일 경우 _angle을 반환, _min 이하일경우 _min을 반환, _max이상일 경우 _max를 반환함.
    }

    private void RotateToKeyboard(ref float _eulerAngleZ)
    {

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

    ///ad회전
    //if (Mathf.Abs(inputX) > 0f)
    //    velocityDeg += playerData.rotAccleDeg * Time.deltaTime * -inputX;
    //else
    //    velocityDeg = Mathf.MoveTowards(velocityDeg, 0, playerData.rotAccleDeg * Time.deltaTime);

    //velocityDeg = Mathf.Clamp(velocityDeg, -playerData.rotMaxVelocityDeg, playerData.rotMaxVelocityDeg);

    //destAngleDeg += velocityDeg * Time.deltaTime;
    ////tr.rotation = Quaternion.Euler(Vector3.forward * destAngleDeg);
    //destAngleDeg = Mathf.Clamp(destAngleDeg, -maxAngle, maxAngle);

    //if (Mathf.Abs(destAngleDeg).Equals(maxAngle))
    //    velocityDeg = 0f;




    ///마우스 회전
    //mouseX = Input.GetAxis("Mouse X");
    //    mouseY = Input.GetAxis("Mouse Y");

    //    Debug.Log("IdleX: " + mouseX);
    //    Debug.Log("IdleY: " + mouseY);

    //    UpdateRotate(mouseX, mouseY);
    //protected void UpdateRotate(float _mouseX, float _mouseY, float _angleZ = 0f)
    //{
    //    eulerAngleY += _mouseX * rotCamYAxisSpeed;
    //    eulerAngleX -= _mouseY * rotCamXAxisSpeed;
    //    eulerAngleX = ClampAngle(eulerAngleX, MinAngleX, MaxAngleX);
    //    //playerData.tr.rotation = Quaternion.Lerp(playerData.tr.rotation, Quaternion.Euler(eulerAngleX, eulerAngleY, 0), playerData.rotAccle * Time.deltaTime);

    //    playerData.tr.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0f);

    //    Debug.Log(eulerAngleX);
    //    Debug.Log(eulerAngleY);
    //}

    //protected float ClampAngle(float _angle, float _min, float _max)
    //{
    //    if (_angle < -360) _angle += 360; // angle이 -360보다 작으면 360을 더해줌. 결과적으로 -380이 -20과 같게 계산됨
    //    if (_angle > 360) _angle -= 360;

    //    return Mathf.Clamp(_angle, _min, _max); // 대소를 비교하여 범위 안의 값일 경우 _angle을 반환, _min 이하일경우 _min을 반환, _max이상일 경우 _max를 반환함.
    //}



    private float rollVelocity = 0f;
    private float rollAccel = 0f;
    private float rollMaxVelocity = 0f;
    private float rollMaxAngle = 0f;

    private float eulerAngleX = 0f;
    private float eulerAngleY = 0f;
    private float rotCamXAxisSpeed = 0f;
    private float rotCamYAxisSpeed = 0f;
    private float minAngleX = 0f;
    private float maxAngleX = 0f;
    private Transform playerTr = null;

    private Vector3 rotVec = Vector3.zero;
    private PlayerData playerData = null;
}
