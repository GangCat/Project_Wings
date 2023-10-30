using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        stateMachine = new PlayerStateMachine(playerData);
        DataInit();
    }

    private void Start()
    {
        StartCoroutine(IdleState());
    }

    private void DataInit()
    {
        playerData.rb = GetComponent<Rigidbody>();
        playerData.tr = transform;
        playerData.inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        stateMachine.Update();
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private IEnumerator IdleState()
    {
        while (true)
        {
            if (playerData.inputHandler.GetInputZ() != 0)
            {
                stateMachine.ChangeState(E_State.MOVE);
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator MoveState()
    {
        if(playerData.rb.velocity.z == 0)
        {
            stateMachine.ChangeState(E_State.IDLE);
            yield break;
        }
        yield return null;
    }


    private PlayerStateMachine stateMachine;

    [SerializeField]
    private PlayerData playerData;

}
