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

    private IEnumerator Test()
    {
        while (true)
        {
            if (Mathf.Abs(data.input.InputX) > 0)
            {
                if (data.input.InputX > 0)
                {
                    if (data.currentMousePos.x > 0)
                        tr.localRotation = Quaternion.Euler(Vector3.right * 45);
                    else
                        tr.localRotation = Quaternion.Euler(Vector3.right * -45);
                }
                else if (data.input.InputX < 0)
                {
                    if (data.currentMousePos.x > 0)
                        tr.localRotation = Quaternion.Euler(Vector3.right * -45);
                    else
                        tr.localRotation = Quaternion.Euler(Vector3.right * 45);
                }
                else
                    tr.localRotation = Quaternion.identity;
            }
            else
            {
                if (data.input.InputZ > 0)
                {
                    tr.localRotation = Quaternion.Euler(Vector3.right * 45);
                }
                else if (data.input.InputZ < 0)
                    tr.localRotation = Quaternion.Euler(Vector3.right * -15);
                else
                    tr.localRotation = Quaternion.identity;
            }

            yield return null;
        }
    }




    private IEnumerator Test2()
    {
        while (true)
        {
            if (Mathf.Abs(data.currentRotZ) <= 30)
            {
                mousePosRatio = 0;
                if (data.input.InputZ > 0)
                {
                    InputZRot = 45;
                }
                else if (data.input.InputZ < 0)
                    InputZRot = -15;
                else
                    InputZRot = 0;
            }
            else if(data.currentRotZ < 0)
            {
                InputZRot = 0;
                mousePosRatio = 1;
                if (data.currentMousePos.x < 0)
                    mousePosRatio = 1 + -(data.currentMousePos.x / 100);
            }
            else{
                InputZRot = 0;
                mousePosRatio = 1;
                if (data.currentMousePos.x > 0)
                    mousePosRatio = 1+(data.currentMousePos.x / 100);
            }
            resultRot = InputZRot + (45 * mousePosRatio);
            currentRot = Mathf.Lerp(currentRot, resultRot, smoothness * Time.deltaTime);
            
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

    private Transform tr = null;
    private PlayerData data = null;

    private Vector2 mousePos = Vector2.zero;
}
