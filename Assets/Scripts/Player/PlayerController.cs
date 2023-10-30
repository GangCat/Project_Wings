using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        stateMachine = new PlayerStateMachine(playerData);
        DataInit();
    }

    private void DataInit()
    {
        playerData.rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        stateMachine.Update();
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }


    private PlayerInputHandler inputHandler; //�Է� �׽�Ʈ ��
    private PlayerStateMachine stateMachine;

    [SerializeField]
    private PlayerData playerData;

}
