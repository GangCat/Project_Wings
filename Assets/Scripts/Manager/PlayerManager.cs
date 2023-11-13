using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    public void Init(VoidIntDelegate _spUpdateCallback)
    {
        input = GetComponent<PlayerInputHandler>();
        playerCtrl = GetComponentInChildren<PlayerController>();
        playerData.input = input;

        playerCtrl.Init(playerData, _spUpdateCallback);
        pmc.Init(playerData);
    }

    private void FixedUpdate()
    {
        pmc.PlayerModelRotate();
    }



    [SerializeField]
    private PlayerData playerData = null;

    private PlayerController playerCtrl = null;
    private PlayerInputHandler input = null;

    [SerializeField]
    private PlayerModelRotateController pmc = null;

    public PlayerData PData => playerData;

}
