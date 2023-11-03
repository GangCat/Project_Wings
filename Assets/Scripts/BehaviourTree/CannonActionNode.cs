using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CannonActionNode : ActionNode
{
    [SerializeField]
    private float radious;
    [SerializeField]
    private float duration;
    [SerializeField]
    private float attackMinHeight;
    [SerializeField]
    private float attackMaxHeight;
    [SerializeField]
    private float term;
    [SerializeField]
    private int cannonBallCnt;
    [SerializeField]
    private GameObject cannonBallPrefab;
    [SerializeField]
    private float cannonBallSpeed;
    [SerializeField]
    private GameObject attackAreaPrefab;

    private float startTime;
    private float lastAttackTime;
    private Transform playerTr;
    protected override void OnStart() {
        startTime = Time.time;
        lastAttackTime = 0;
        playerTr = context.playerTr;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        
        if(Time.time - startTime <= duration)
        {
            if (Time.time - lastAttackTime > term)
            {
                Destroy(Instantiate(attackAreaPrefab, playerTr.position, Quaternion.identity),3f);
                for (int i=0; i < cannonBallCnt; ++i)
                {
                    Vector2 rnd = Random.insideUnitCircle * radious;
                    Vector3 spawnPositionWithHeight = playerTr.position + new Vector3(rnd.x, Random.Range(attackMinHeight, attackMaxHeight), rnd.y);
                    GameObject bullet = Instantiate(cannonBallPrefab, spawnPositionWithHeight, Quaternion.identity);
                    bullet.GetComponent<CannonBallController>().Init(cannonBallSpeed);
                    lastAttackTime = Time.time;
                }
            }
            return State.Running;
        }

        return State.Success;

        
    }
}
