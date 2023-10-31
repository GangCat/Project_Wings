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
        ChangeCoroutine(IdleState());
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


    private void ChangeCoroutine(IEnumerator _Coroutine)
    {
        StopAllCoroutines();
        StartCoroutine(_Coroutine);
    }

    private IEnumerator IdleState()
    {
        while (true)
        {
            if (playerData.inputHandler.GetInputZ() != 0)
            {
                stateMachine.ChangeState(E_State.MOVE);
                ChangeCoroutine(MoveState());
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator MoveState()
    {
        while (true)
        {
            if (playerData.isMove == false && playerData.inputHandler.GetInputZ() == 0)
            {
                stateMachine.ChangeState(E_State.IDLE);
                ChangeCoroutine(IdleState());
                Debug.Log("IDLE");
                yield break;
            }
            yield return null;
        }
    }


    private PlayerStateMachine stateMachine;

    [SerializeField]
    private PlayerData playerData;

}
