using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public void Init()
    {
        mainCam = GetComponentInChildren<Camera>();
    }

    private Camera mainCam = null;
}
