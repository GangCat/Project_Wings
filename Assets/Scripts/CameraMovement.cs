using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public void Init(Transform _playerTr)
    {
        playerTr = _playerTr;
    }

    private void Update()
    {



    }

    private void LateUpdate()
    {


        desiredRotation = Quaternion.LookRotation(playerTr.forward);
        quaternion = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeed);
        Pos = playerTr.position + -playerTr.forward * offset + transform.up * 3f;
        transform.rotation = quaternion;
        transform.position = Pos;
    }

    [SerializeField]
    private float offset = 0f;

    public float smoothSpeed = 0.125f;

    Quaternion desiredRotation = Quaternion.identity;
    Quaternion quaternion = Quaternion.identity;
    private Transform playerTr = null;
    private Vector3 rotAimPos = Vector3.zero;

    private Vector3 Pos = Vector3.zero;
}
