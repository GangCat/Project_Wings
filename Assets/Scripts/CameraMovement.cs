using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public void Init(Transform _playerTr)
    {
        playerTr = _playerTr;
    }

    private void LateUpdate()
    {
        transform.position = playerTr.position + -playerTr.forward * offset + transform.up * 3f;
        //rotAimPos = playerTr.position;
        //rotAimPos += playerTr.up * 3f;
        //transform.rotation = Quaternion.LookRotation(playerTr.position - transform.position);
        transform.rotation = Quaternion.LookRotation(playerTr.forward);
    }

    [SerializeField]
    private float offset = 0f;

    private Transform playerTr = null;
    private Vector3 rotAimPos = Vector3.zero;
}
