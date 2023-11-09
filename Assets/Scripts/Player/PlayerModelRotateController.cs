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
        if (playerData.currentMoveSpeed < 5 || playerData.input.InputZ <= 0)
        {
 //           rotZ = Mathf.MoveTowards(rotZ, 0, playerData.rollReturnAccel * Time.deltaTime);
        }
        else
        {
            RotateToKeyboardZ(ref rotZ);
        }
        //rotation.y = rotation.z;
        playerData.currentRotZ = currentRotZ;

//        tr.localRotation = Quaternion.Euler(rotation);
//       tr.rotation *= Quaternion.Euler(Vector3.forward * rotZ);


        
        // 로컬 회전 적용
//        transform.Rotate(rotation, Space.Self);

        // 월드 회전 적용
        

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
        float eulerAngleX;
        if (Mathf.Abs(playerData.input.InputX) > 0f)
            rollVelocity += rollAccel * Time.deltaTime * -playerData.input.InputX;
        else
            rollVelocity = Mathf.MoveTowards(rollVelocity, 0, rollAccel * Time.deltaTime);

        rollVelocity = Mathf.Clamp(rollVelocity, -rollMaxVelocity, rollMaxVelocity);

        currentRotX += rollVelocity * Time.deltaTime;
        eulerAngleX = rollVelocity * Time.deltaTime;
        currentRotX = Mathf.Clamp(currentRotZ, -rollMaxAngle, currentMaxAngle);

        if (Mathf.Abs(currentRotZ).Equals(currentMaxAngle))
        {
            rollVelocity = 0f;
            eulerAngleX = 0f;
        }
        transform.Rotate(tr.right * eulerAngleX, Space.World);
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


    private float rollVelocity = 0f;
    private float rollAccel = 0f;
    private float rollMaxVelocity = 0f;
    private float rollMaxAngle = 0f;


    private Transform tr = null;
    private PlayerData playerData = null;

    private Vector2 mousePos = Vector2.zero;
}
