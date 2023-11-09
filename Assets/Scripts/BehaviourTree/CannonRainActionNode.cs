using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System.Buffers;

public class CannonRainActionNode : ActionNode
{
    [SerializeField]
    private float bossOffSet = 2000;
    [SerializeField]
    private Vector2 mapRadious = new Vector2(5000, 5000);
    [SerializeField]
    private float attackMinHeight = 500;
    [SerializeField]
    private float attackMaxHeight = 5000;
    [SerializeField]
    private int cannonBallCnt = 1000;
    [SerializeField]
    private GameObject cannonBallPrefab = null;
    [SerializeField]
    private float cannonBallSpeed = 100;
    [SerializeField]
    private GameObject attackAreaPrefab = null;

    private float startTime;
    private float lastAttackTime;
    private Vector3 rndAttackPos;
    private Vector2 rnd1;


    protected override void OnStart()
    {
        startTime = Time.time;

        
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        
        for (int i = 0; i < cannonBallCnt; ++i)
            {
                rnd1.x = Random.Range(-1.0f, 1.0f) * bossOffSet;
                rnd1.y = Random.Range(-1.0f, 1.0f) * bossOffSet;
                if (rnd1.x < 0) mapRadious.x *= -1;
                if (rnd1.y < 0) mapRadious.y *= -1;

                rndAttackPos = new Vector3(Random.Range(rnd1.x * bossOffSet, mapRadious.x), 0, Random.Range(rnd1.y * bossOffSet, mapRadious.y));
                Destroy(Instantiate(attackAreaPrefab, rndAttackPos, Quaternion.identity), 15f);
                Vector3 spawnPositionWithHeight = rndAttackPos + new Vector3(0, Random.Range(attackMinHeight, attackMaxHeight), 0);

                GameObject bullet =  context.cannonRainMemoryPool.ActivateCannonBall();
                bullet.GetComponent<CannonBallController>().Init(cannonBallSpeed, spawnPositionWithHeight, context.cannonRainMemoryPool);

                lastAttackTime = Time.time;
                

                //GameObject bullet = Instantiate(cannonBallPrefab, spawnPositionWithHeight, Quaternion.identity);
            }

        return State.Success;


    }
}
