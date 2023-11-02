using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class GatilingActionNode : ActionNode
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float maxBulletCnt;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float fireRate;


    private Transform playerTr;
    private Transform gunMuzzleTr;
    private GameObject gatlingHolder;
    private float curBulletCnt;
    private float lastFireTime;


    protected override void OnStart() {
        curBulletCnt = maxBulletCnt;
        playerTr = context.playerTr;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        RotateTurretToPlayer();

        if (CanFire())
        {
            FireBullet();
        }
        return State.Success;
    }
    private void RotateTurretToPlayer()
    {
        if (playerTr != null)
        {
            Vector3 playerDirection = new Vector3(playerTr.position.x - gatlingHolder.transform.position.x, 0f, playerTr.position.z - gatlingHolder.transform.position.z);
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);

            // 부드럽게 회전하기 위해 Lerp 사용
            gatlingHolder.transform.rotation = Quaternion.Slerp(gatlingHolder.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private bool CanFire()
    {
        return curBulletCnt > 0 && Time.time - lastFireTime >= 1 / fireRate;
    }

    private void FireBullet()
    {
        lastFireTime = Time.time;
        curBulletCnt--;

        Vector3 tmp = gunMuzzleTr.up;

        //float angle = Random.Range(0, 360);
        //float radians = angle * Mathf.Deg2Rad;
        //Vector3 spawnPosition = playerTr.position + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)) * randomRange;

        Quaternion rot = Quaternion.AngleAxis(Random.Range(0, 360), gunMuzzleTr.forward);
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
        Vector3 targetPos = rotationMatrix.MultiplyPoint3x4(tmp) + playerTr.position;

        GameObject bullet = Instantiate(bulletPrefab, gunMuzzleTr.position, gunMuzzleTr.rotation);
    }
}
