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

    private void FixedUpdate()
    {
          float smoothedPosX = Mathf.Lerp(transform.position.x, Pos.x, posSmoothingX * Time.deltaTime);
        float smoothedPosY = Mathf.Lerp(transform.position.y, Pos.y, posSmoothingY * Time.deltaTime);
        float smoothedPosZ = Mathf.Lerp(transform.position.z, Pos.z, posSmoothingZ * Time.deltaTime);
        transform.position = new Vector3(smoothedPosX, smoothedPosY, smoothedPosZ);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 desiredRotation = Quaternion.LookRotation(playerTr.forward).eulerAngles;

        float xRot = Mathf.SmoothDampAngle(currentRotation.x, desiredRotation.x, ref rotVectorVelocity.x, smoothSpeed * Time.deltaTime);
        float yRot = Mathf.SmoothDampAngle(currentRotation.y, desiredRotation.y, ref rotVectorVelocity.y, smoothSpeed* Time.deltaTime);
        float zRot = Mathf.SmoothDampAngle(currentRotation.z, desiredRotation.z, ref rotVectorVelocity.z, smoothSpeed* Time.deltaTime);

        quaternion = Quaternion.Euler(new Vector3(xRot, yRot, zRot));

        Pos = playerTr.position - playerTr.forward * offset + transform.up * 3f;

        // 부드러운 위치 보간
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, Pos, ref posVelocity, smoothSpeed);

        transform.rotation = quaternion;
    }

    private void LateUpdate()
    {


      
        //transform.position = Pos;


        //desiredRotation = Quaternion.LookRotation(playerTr.forward);
        //quaternion = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeed);
        //Pos = playerTr.position + -playerTr.forward * offset + transform.up * 3f;
        //transform.rotation = quaternion;
        //transform.position = Pos;
    }
    public float posSmoothingX = 0.3f;
    public float posSmoothingY = 0.3f;
    public float posSmoothingZ = 0.3f;

    [SerializeField]
    private float offset = 0f;

    public float smoothSpeed = 0.125f;

    Quaternion desiredRotation = Quaternion.identity;
    Quaternion quaternion = Quaternion.identity;
    private Transform playerTr = null;
    private Vector3 rotAimPos = Vector3.zero;

    private Vector3 Pos = Vector3.zero;

    private Vector3 rotVectorVelocity;

    private Vector3 posVelocity;
}
