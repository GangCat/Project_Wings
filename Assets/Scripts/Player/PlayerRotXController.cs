using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotXController : MonoBehaviour
{
    [SerializeField]
    private Transform tr;
    [SerializeField]
    private PlayerData playerData;
    public float smooth;

    float currentXRotation = 0f;
    float targetRotateX;
    float calcRotateX;
    private void Update()
    {
        targetRotateX = tr.rotation.eulerAngles.x;
        Debug.Log(targetRotateX);
        if (targetRotateX >= 250)
        {
            calcRotateX = 360-targetRotateX;
        } else if(targetRotateX >= 5 && targetRotateX <= 100)
        {
            calcRotateX = targetRotateX * (playerData.currentMoveSpeed/playerData.moveForwardVelocityLimit);
        
        }

        currentXRotation = Mathf.Lerp(currentXRotation, calcRotateX, smooth*Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);

    }
}
