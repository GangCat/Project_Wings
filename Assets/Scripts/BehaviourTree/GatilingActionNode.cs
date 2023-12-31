using UnityEngine;
using TheKiwiCoder;

public class GatilingActionNode : ActionNode
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private int maxBulletCnt = 80;
    [SerializeField]
    private float rotationSpeed = 30f;
    [SerializeField]
    private float fireRate = 10f;
    [SerializeField]
    private float headRotationSpeed = 20f;
    [SerializeField]
    private float rebound = 2f;
    [SerializeField]
    private float autoDestroyTime = 20f;


    private Transform playerTr;
    private Transform gunMuzzleTr;
    private GameObject gatlingHolder;

    private float curBulletCnt;
    private float lastFireTime;
    private float diffY;
    private float cetha;
    private Vector3 rndRebound;

    protected override void OnStart() {
        curBulletCnt = maxBulletCnt;
        playerTr = context.playerTr;
        gunMuzzleTr = context.gunMuzzleTr;
        gatlingHolder = context.gatlingHolderGo;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        rndRebound = new Vector3(Random.Range(-rebound, rebound), Random.Range(-rebound, rebound), Random.Range(-rebound, rebound));
        RotateTurretToPlayer();
        RotateTurretHeadToPlayer();

        if (CanFire())
        {
            FireBullet();
        }
        
        if (curBulletCnt > 0)
            return State.Running;

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

    private void RotateTurretHeadToPlayer()
    {
        if (context.playerTr != null)
        {
            diffY = playerTr.position.y - context.gatlingHeadGo.transform.position.y;
            cetha = Mathf.Asin(diffY / Vector3.Distance(playerTr.position, context.gatlingHeadGo.transform.position)) * Mathf.Rad2Deg;
            context.gatlingHeadGo.transform.localRotation = Quaternion.Euler(Vector3.left * cetha);

            //Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            // 부드럽게 회전하기 위해 Lerp 사용
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private bool CanFire()
    {
        return curBulletCnt > 0 && Time.time - lastFireTime >= 1 / fireRate;
    }

    private void FireBullet()
    {
        lastFireTime = Time.time;
        //curBulletCnt--;
        --curBulletCnt;

        Vector3 tmp = gunMuzzleTr.up;

        //float angle = Random.Range(0, 360);
        //float radians = angle * Mathf.Deg2Rad;
        //Vector3 spawnPosition = playerTr.position + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)) * randomRange;

        Quaternion rot = Quaternion.AngleAxis(Random.Range(0, 360), gunMuzzleTr.forward);
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
        Vector3 targetPos = rotationMatrix.MultiplyPoint3x4(tmp) + playerTr.position;

        GameObject bullet = Instantiate(bulletPrefab, gunMuzzleTr.position, gunMuzzleTr.rotation * Quaternion.Euler(rndRebound));
        bullet.GetComponent<BulletController>().Init(autoDestroyTime);
    }
}
