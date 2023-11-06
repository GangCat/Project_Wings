using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public void Init(Transform _playerTr,PlayerData _playerData)
    {
        cam = GetComponent<Camera>();
        mainCam = GetComponentInChildren<CameraMovement>();
        mainCam.Init(_playerTr, _playerData);
    }

    private CameraMovement mainCam = null;
    private Camera cam = null;


    private IEnumerator ChangeFOV(Camera camera, float targetFOV, float duration)
    {
        float startFOV = camera.fieldOfView;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            camera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        camera.fieldOfView = targetFOV;
    }

}
