using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    private void Awake()
    {
        // 나중에 이렇게 게으른 초기화로 할거임
        // public void Init();
        input = GetComponent<PlayerInputHandler>();
        playerCtrl = GetComponentInChildren<PlayerController>();
        virtualMouse = GetComponentInChildren<VirtualMouse>();  // 플레이어의 조준점

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
