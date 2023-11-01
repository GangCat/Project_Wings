using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotateController : MonoBehaviour
{
    public void Init(PlayerData _data)
    {
        tr = GetComponent<Transform>();
        data = _data;

        StartCoroutine("Test");
    }

    private IEnumerator Test()
    {
        while (true)
        {
            if (Mathf.Abs(data.input.InputX) > 0)
            {
                if (data.input.InputX > 0)
                {
                    if (data.mousePos.x > 0)
                        tr.localRotation = Quaternion.Euler(Vector3.right * 45);
                    else
                        tr.localRotation = Quaternion.Euler(Vector3.right * -45);
                }
                else if (data.input.InputX < 0)
                {
                    if (data.mousePos.x > 0)
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

    private Transform tr = null;
    private PlayerData data = null;

    private Vector2 mousePos = Vector2.zero;
}
