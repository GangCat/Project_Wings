using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public void Init(Transform _playerTr)
    {
        mainCam = GetComponentInChildren<CameraMovement>();
        mainCam.Init(_playerTr);
    }

    private CameraMovement mainCam = null;
}
