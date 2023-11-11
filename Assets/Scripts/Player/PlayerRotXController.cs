using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotXController : MonoBehaviour
{
    [SerializeField]
    private Transform tr;

    private void Update()
    {
        // 현재 X 축 회전을 읽어옴
        float currentXRotation = tr.rotation.eulerAngles.x;
        Debug.Log(currentXRotation);
        // 만약 X 축 회전이 0 이하일 때, 부정값으로 변경
        if (currentXRotation >= 280)
        {
            transform.localRotation = Quaternion.Euler(-currentXRotation, 0f, 0f);
        }
    }
}
