using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.SubsystemsImplementation;

public class FootWindPushActionNode : ActionNode
{
    [SerializeField]
    private float increasingScaleSpeed= 5f;
    [SerializeField]
    private float duration =5f;

    private GameObject footWindGo;
    protected override void OnStart() {
        Debug.Log("start");
        footWindGo = Instantiate(context.footWindGo,context.footWindTr);
    }

    protected override void OnStop() {
        Destroy(footWindGo);
    }

    protected override State OnUpdate() {
        
        while(duration > 0f)
        {
            footWindGo.transform.localScale += new Vector3(increasingScaleSpeed * Time.deltaTime, 0, increasingScaleSpeed * Time.deltaTime);
            duration -= Time.deltaTime;
        }
        return State.Success;
    }
}
