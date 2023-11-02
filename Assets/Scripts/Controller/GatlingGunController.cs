using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 위치를 찾는다.
/// 총구를 플레이어 위치까지 돌릴 수 있는 속도가 잇다.
/// 현재 총구의 각도를 플레이어 위치까지 돌린다. 단, lerp를 사용해서 부드럽게 돌려야한다.
/// 플레이어 위치에 도달했을 경우 총알 프리팹을 발사한다.
/// 총알 발사 갯수가 있다.
/// 총알을 최대 발사 갯수까지 생성한다.
/// </summary>
public class GatlingGunController : MonoBehaviour
{
    public Transform player; // 플레이어 위치를 저장하는 Transform
    public Transform gunMuzzle; // 총구 위치
    public GameObject bulletPrefab; // 총알 프리팹

    public float rotationSpeed = 5.0f; // 터렛 회전 속도
    public int maxBulletCount = 10; // 최대 총알 발사 갯수
    public float fireRate = 1.0f; // 총알 발사 속도 (초당 발사 횟수)

    private int currentBulletCount; // 현재 총알 발사 갯수
    private float lastFireTime; // 마지막 총알 발사 시간


    private void Start()
    {
        currentBulletCount = maxBulletCount;
        lastFireTime = 0f;
    }

    private void Update()
    {
        RotateTurretToPlayer();

        if (CanFire())
        {
            FireBullet();
        }
    }

    private void RotateTurretToPlayer()
    {
        if (player != null)
        {
            Vector3 playerDirection = player.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);

            // 부드럽게 회전하기 위해 Lerp 사용
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 총구의 각도 설정
            gunMuzzle.LookAt(player);
        }
    }

    private bool CanFire()
    {
        return currentBulletCount > 0 && Time.time - lastFireTime >= 1 / fireRate;
    }

    private void FireBullet()
    {
        lastFireTime = Time.time;
        currentBulletCount--;

        GameObject bullet = Instantiate(bulletPrefab, gunMuzzle.position, gunMuzzle.rotation);
    }
}
