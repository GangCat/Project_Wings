using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotateController : MonoBehaviour
{
    public void Init(PlayerData _data)
    {
        tr = GetComponent<Transform>();
        data = _data;

        StartCoroutine("Test2");
    }


    private IEnumerator Test2()
    {
        while (true)
        {
            currentMoveVelocityRatio = data.currentMoveSpeed/data.moveForwardVelocityLimit;
            InputZRot = 45 * currentMoveVelocityRatio;
            if (Mathf.Abs(data.currentRotZ) <= 30)
            {
                mousePosRatio = 1;
            }
            else if(data.currentRotZ < 0)
            {
                if (data.currentMousePos.x < 0)
                    mousePosRatio = 1 + -(data.currentMousePos.x / 100);
            }
            else{
                if (data.currentMousePos.x > 0)
                    mousePosRatio = 1+(data.currentMousePos.x / 100);
            }
            resultRot = InputZRot * (mousePosRatio);
            currentRot = Mathf.Lerp(currentRot, resultRot, smoothness * Time.deltaTime);
            currentRot = Mathf.Clamp(currentRot, -90, 90);

            Rotation = new Vector3(currentRot, 0, 0);
            tr.localRotation = Quaternion.Euler(Rotation);

            yield return null;
        }
    }

    private Vector3 Rotation;

    private float currentRot;
    private float resultRot;
    private float smoothness = 10f;

    private float InputZRot;
    private float mousePosRatio;

    private float currentMoveVelocityRatio;

    private Transform tr = null;
    private PlayerData data = null;

    private Vector2 mousePos = Vector2.zero;
}
