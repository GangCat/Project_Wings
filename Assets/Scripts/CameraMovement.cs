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
        float smoothedPosX = Mathf.Lerp(transform.position.x, Pos.x, posSmoothX * Time.deltaTime);
        float smoothedPosY = Mathf.Lerp(transform.position.y, Pos.y, posSmoothY * Time.deltaTime);
        float smoothedPosZ = Mathf.Lerp(transform.position.z, Pos.z, posSmoothZ * Time.deltaTime);
        transform.position = new Vector3(smoothedPosX, smoothedPosY, smoothedPosZ);

        currentRotation = transform.rotation.eulerAngles;
        desiredRotation = Quaternion.LookRotation(playerTr.forward).eulerAngles;

        float xRot = Mathf.SmoothDampAngle(currentRotation.x, desiredRotation.x, ref rotVectorVelocity.x, rotSmoothX * Time.deltaTime);
        float yRot = Mathf.SmoothDampAngle(currentRotation.y, desiredRotation.y, ref rotVectorVelocity.y, rotSmoothY * Time.deltaTime);
        float zRot = Mathf.SmoothDampAngle(currentRotation.z, desiredRotation.z, ref rotVectorVelocity.z, rotSmoothZ * Time.deltaTime);

        quaternion = Quaternion.Euler(new Vector3(xRot, yRot, zRot));

        Pos = playerTr.position - playerTr.forward * offset + transform.up * 3f;
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

    public float posSmoothX = 10f;
    public float posSmoothY = 10f;
    public float posSmoothZ = 10f;

    public float rotSmoothX = 0.1f;
    public float rotSmoothY = 0.05f;
    public float rotSmoothZ = 0.1f;



    [SerializeField]
    private float offset = 0f;

    private Vector3 currentRotation = Vector3.zero;
    private Vector3 desiredRotation = Vector3.zero;
    private Vector3 Pos = Vector3.zero;
    private Vector3 rotVectorVelocity;
    private Quaternion quaternion = Quaternion.identity;
    private Transform playerTr = null;
}
