using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� ��ġ�� ã�´�.
/// �ѱ��� �÷��̾� ��ġ���� ���� �� �ִ� �ӵ��� �մ�.
/// ���� �ѱ��� ������ �÷��̾� ��ġ���� ������. ��, lerp�� ����ؼ� �ε巴�� �������Ѵ�.
/// �÷��̾� ��ġ�� �������� ��� �Ѿ� �������� �߻��Ѵ�.
/// �Ѿ� �߻� ������ �ִ�.
/// �Ѿ��� �ִ� �߻� �������� �����Ѵ�.
/// </summary>
public class GatlingGunController : MonoBehaviour
{
    public Transform player; // �÷��̾� ��ġ�� �����ϴ� Transform
    public Transform gunMuzzle; // �ѱ� ��ġ
    public GameObject bulletPrefab; // �Ѿ� ������

    public float rotationSpeed = 5.0f; // �ͷ� ȸ�� �ӵ�
    public int maxBulletCount = 10; // �ִ� �Ѿ� �߻� ����
    public float fireRate = 1.0f; // �Ѿ� �߻� �ӵ� (�ʴ� �߻� Ƚ��)

    private int currentBulletCount; // ���� �Ѿ� �߻� ����
    private float lastFireTime; // ������ �Ѿ� �߻� �ð�


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

            // �ε巴�� ȸ���ϱ� ���� Lerp ���
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // �ѱ��� ���� ����
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
