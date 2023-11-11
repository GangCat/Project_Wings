using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotXController : MonoBehaviour
{
    [SerializeField]
    private Transform tr;

    private void Update()
    {
        // ���� X �� ȸ���� �о��
        float currentXRotation = tr.rotation.eulerAngles.x;
        Debug.Log(currentXRotation);
        // ���� X �� ȸ���� 0 ������ ��, ���������� ����
        if (currentXRotation >= 280)
        {
            transform.localRotation = Quaternion.Euler(-currentXRotation, 0f, 0f);
        }
    }
}
