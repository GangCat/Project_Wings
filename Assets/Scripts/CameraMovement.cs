using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public void Init(Transform _playerTr,PlayerData _playerData)
    {
        playerTr = _playerTr;
        playerData = _playerData;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ���콺 ��Ŭ���� ó�� ���� ��
        {
            cameraYaw = currentRotation.y;
            cameraPitch = currentRotation.x;
            playerData.isFreeLock = true; // ������ Ȱ��ȭ

        }
        else if (Input.GetMouseButtonUp(1)) // ���콺 ��Ŭ���� ���� ��
        {
            playerData.isFreeLock = false; // ������ ��Ȱ��ȭ


        }
        if (playerData.isFreeLock)
        {

        }
    }

    private void FixedUpdate()
    {
        if (!playerData.isFreeLock)
        {
            FollowPlayerRot();
            FollowPlayerPos();
        }
        else
        {
            FreeLock();
        }

        transform.rotation = quaternion;
        transform.position = cameraPos;
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

    private void FollowPlayerPos()
    {
        calcPos = playerTr.position - playerTr.forward * offset + transform.up * 3f;
        float smoothedPosX = Mathf.Lerp(transform.position.x, calcPos.x, posSmoothX * Time.deltaTime);
        float smoothedPosY = Mathf.Lerp(transform.position.y, calcPos.y, posSmoothY * Time.deltaTime);
        float smoothedPosZ = Mathf.Lerp(transform.position.z, calcPos.z, posSmoothZ * Time.deltaTime);
        cameraPos = new Vector3(smoothedPosX, smoothedPosY, smoothedPosZ);
    }

    private void FollowPlayerPos(Vector3 _playerPos)
    {
        float smoothedPosX = Mathf.Lerp(transform.position.x, _playerPos.x, posSmoothX * Time.deltaTime);
        float smoothedPosY = Mathf.Lerp(transform.position.y, _playerPos.y, posSmoothY * Time.deltaTime);
        float smoothedPosZ = Mathf.Lerp(transform.position.z, _playerPos.z, posSmoothZ * Time.deltaTime);
        cameraPos = new Vector3(smoothedPosX, smoothedPosY, smoothedPosZ);
    }

    private void FollowPlayerRot()
    {
        currentRotation = transform.rotation.eulerAngles;
        desiredRotation = Quaternion.LookRotation(playerTr.forward).eulerAngles;

        float xRot = Mathf.SmoothDampAngle(currentRotation.x, desiredRotation.x, ref rotVectorVelocity.x, rotSmoothX * Time.deltaTime);
        float yRot = Mathf.SmoothDampAngle(currentRotation.y, desiredRotation.y, ref rotVectorVelocity.y, rotSmoothY * Time.deltaTime);
        float zRot = Mathf.SmoothDampAngle(currentRotation.z, desiredRotation.z, ref rotVectorVelocity.z, rotSmoothZ * Time.deltaTime);

        quaternion = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
    }

    private bool CameraRay() // ��ֹ� ī�޶� �ɸ��� ������� �ϴ°ǵ� ���̾� ������ �ȵ��־ �̻��� ���� ����.
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerTr.position - transform.position, out hit))
        {
            Debug.DrawRay(transform.position, playerTr.position - transform.position, Color.red);
            if (hit.collider.gameObject != playerTr.gameObject)
            {
                transform.position = hit.point;
                return true;
            }
        }
        return false;
    }

    private void FreeLock()
    {
        cameraYaw += Input.GetAxis("Mouse X") * 300f * Time.deltaTime;
        cameraPitch -= Input.GetAxis("Mouse Y") * 300f * Time.deltaTime;

        // ī�޶� Pitch ������ ����
        //cameraPitch = Mathf.Clamp(cameraPitch, pitchMinMax.x, pitchMinMax.y);

        // ī�޶��� ȸ���� ����
        quaternion = Quaternion.Euler(cameraPitch, cameraYaw, 0);
        Vector3 CalcPlayerPos = playerTr.position - (quaternion * Vector3.forward) * offset + transform.up * 3f;
        cameraPos = CalcPlayerPos;
        //FollowPlayerPos(CalcPlayerPos);

    }


    private float currentAngle = 0.0f;
    private Vector3 cameraOffset;
    private float cameraYaw; 
    private float cameraPitch;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

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
    private Vector3 calcPos = Vector3.zero;
    private Vector3 cameraPos = Vector3.zero;
    private Vector3 rotVectorVelocity;
    private Quaternion quaternion = Quaternion.identity;
    private Transform playerTr = null;
    private PlayerData playerData = null;

}
