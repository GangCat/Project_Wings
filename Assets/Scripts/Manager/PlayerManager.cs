using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    private void Awake()
    {
        // ���߿� �̷��� ������ �ʱ�ȭ�� �Ұ���
        // public void Init();
        input = GetComponent<PlayerInputHandler>();
        playerCtrl = GetComponentInChildren<PlayerController>();
        virtualMouse = GetComponentInChildren<VirtualMouse>();  // �÷��̾��� ������

        playerData.input = input;

        playerCtrl.Init(playerData);
        //virtualMouse.Init(playerData);
    }



    [SerializeField]
    private PlayerData playerData = null;

    private PlayerController playerCtrl = null;
    private PlayerInputHandler input = null;

    private VirtualMouse virtualMouse;
}
