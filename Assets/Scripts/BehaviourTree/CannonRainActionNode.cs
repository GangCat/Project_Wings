using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System.Buffers;

public class CannonRainActionNode : ActionNode
{
    [SerializeField]
    private float bossOffSet = 300;
    [SerializeField]
    private Vector2 mapRadious = new Vector2(1500,1500);
    [SerializeField]
    private float attackMinHeight = 10000;
    [SerializeField]
    private float attackMaxHeight = 15000;
    [SerializeField]
    private int cannonBallCnt = 20;
    [SerializeField]
    private GameObject cannonBallPrefab = null;
    [SerializeField]
    private float cannonBallSpeed = 2000;
    [SerializeField]
    private GameObject attackAreaPrefab = null;
    [SerializeField]
    private float patternFinishTime = 10f;

    private Vector3 rndAttackPos;
    private Vector2 rnd1;
    private float startTime = 0f;

    protected override void OnStart()
    {
        startTime = Time.time;

        for (int i = 0; i < cannonBallCnt; ++i)
        {
            rnd1.x = Random.Range(-1.0f, 1.0f) * bossOffSet;
            rnd1.y = Random.Range(-1.0f, 1.0f) * bossOffSet;
            if (rnd1.x < 0) mapRadious.x *= -1;
            if (rnd1.y < 0) mapRadious.y *= -1;

            rndAttackPos = new Vector3(Random.Range(rnd1.x, mapRadious.x), 0, Random.Range(rnd1.y, mapRadious.y));
            Vector3 spawnPositionWithHeight = rndAttackPos + new Vector3(0, Random.Range(attackMinHeight, attackMaxHeight), 0);
            GameObject bullet = context.cannonRainMemoryPool.ActivateCannonBall();
            bullet.GetComponent<CannonRainBallController>().Init(cannonBallSpeed, spawnPositionWithHeight, context.cannonRainMemoryPool, attackAreaPrefab);
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (Time.time - startTime > patternFinishTime)
            return State.Success;

        return context.cannonRainMemoryPool.IsActiveItemEmpty() ? State.Success : State.Running;
    }
}
