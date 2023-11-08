using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotateController : MonoBehaviour
{
    public void Init(PlayerData _data)
    {
        tr = GetComponent<Transform>();
        playerData = _data;

        StartCoroutine("Test2");
    }

    public void PlayerModelRotate()
    {
     //   tr.localRotation = Quaternion.Euler(rotation);
        if (playerData.currentMoveSpeed < 5 || playerData.input.InputZ <= 0)
        {
 //           rotZ = Mathf.MoveTowards(rotZ, 0, playerData.rollReturnAccel * Time.deltaTime);
        }
        else
        {
            RotateToKeyboardZ(ref rotZ);
            //RotateToSpeedX();
        }
        //rotation.y = rotation.z;
        playerData.currentRotZ = currentRotZ;

        

    }

    private void RotateToKeyboardZ(ref float _eulerAngleZ)
    {
        rollAccel = playerData.rollAccel;
        rollMaxVelocity = playerData.rollMaxVelocity;
        rollMaxAngle = playerData.rollMaxAngle;

        if (Mathf.Abs(playerData.input.InputX) > 0f)
            rollVelocity += rollAccel * Time.deltaTime * -playerData.input.InputX;
        else
            rollVelocity = Mathf.MoveTowards(rollVelocity, 0, rollAccel * Time.deltaTime);

        rollVelocity = Mathf.Clamp(rollVelocity, -rollMaxVelocity, rollMaxVelocity);

        currentRotZ += rollVelocity * Time.deltaTime;
        _eulerAngleZ = rollVelocity * Time.deltaTime;
        currentRotZ = Mathf.Clamp(currentRotZ, -rollMaxAngle, rollMaxAngle);

        if (Mathf.Abs(currentRotZ).Equals(rollMaxAngle))
        {
            rollVelocity = 0f;
            _eulerAngleZ = 0f;
        }
        transform.Rotate(tr.forward * _eulerAngleZ, Space.World);
    }


    private void RotateToSpeedX()
    {
        pitchMaxVelocity = 90f;
        float eulerAngleX;

        if (Mathf.Abs(playerData.input.InputZ) > 0f)
        {
            pitchVelocity += pitchAccel * Time.deltaTime * playerData.input.InputZ;
            Debug.Log(pitchVelocity);
        }
        else
            pitchVelocity = Mathf.MoveTowards(pitchVelocity, 0, pitchAccel * Time.deltaTime);

        pitchVelocity = Mathf.Clamp(pitchVelocity, -pitchMaxVelocity, pitchMaxVelocity);

        currentRotX += pitchVelocity * Time.deltaTime; // 변수 이름 수정 (currentRotZ -> currentRotX)
        eulerAngleX = pitchVelocity * Time.deltaTime; // 변수 이름 수정 (rollVelocity -> pitchVelocity)
        currentRotX = Mathf.Clamp(currentRotX, -90, 90); // 변수 이름 수정 (currentRotZ -> currentRotX)

        if (Mathf.Abs(currentRotX).Equals(90)) // 변수 이름 수정 (currentRotZ -> currentRotX)
        {
            pitchVelocity = 0f; // 변수 이름 수정 (rollVelocity -> pitchVelocity)
            eulerAngleX = 0f;
        }

        // X 축 주위의 회전을 적용
        transform.Rotate(Vector3.right * eulerAngleX, Space.Self); // 변수 이름 수정 (tr.right -> Vector3.right)
    }

    private IEnumerator Test2()
    {
        while (true)
        {
            currentMoveVelocityRatio = playerData.currentMoveSpeed/playerData.moveForwardVelocityLimit;
            InputZRot = 45 * currentMoveVelocityRatio;
            if (Mathf.Abs(playerData.currentRotZ) <= 30)
            {
                mousePosRatio = 1;
            }
            else if(playerData.currentRotZ < 0)
            {
                if (playerData.currentMousePos.x < 0)
                    mousePosRatio = 1 + -(playerData.currentMousePos.x / 100);
            }
            else{
                if (playerData.currentMousePos.x > 0)
                    mousePosRatio = 1+(playerData.currentMousePos.x / 100);
            }
            resultRot = InputZRot * (mousePosRatio);
            currentMaxAngle = Mathf.Lerp(currentMaxAngle, resultRot, smoothness);
            currentMaxAngle = Mathf.Clamp(currentMaxAngle, -90, 90);

            rotation.x = currentMaxAngle;

            yield return new WaitForFixedUpdate();
        }
    }

    private Vector3 rotation;
    private float rotZ;

    private float currentRotZ;
    private float currentRotX;
    private float currentMaxAngle;
    private float resultRot;
    private float smoothness = 0.1f;

    private float InputZRot;
    private float mousePosRatio;

    private float currentMoveVelocityRatio;

    private float pitchVelocity = 0f;
    private float pitchAccel = 1000f;
    private float pitchMaxVelocity = 0f;
    private float pitchMaxAngle = 90f;



    private float rollVelocity = 0f;
    private float rollAccel = 0f;
    private float rollMaxVelocity = 0f;
    private float rollMaxAngle = 0f;


    private Transform tr = null;
    private PlayerData playerData = null;

    private Vector2 mousePos = Vector2.zero;
}
