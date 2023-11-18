using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_ShieldHpbar : MonoBehaviour
{
    [SerializeField]
    private Image hpBar = null;
    [SerializeField]
    private BossShieldGenerator boss = null;
    private RectTransform rectTr = null;

    void Start()
    {
        rectTr = GetComponent<RectTransform>();
        uiHp = boss.GetCurHp;
    }
    private void Update()
    {
        if (boss != null)
        {
            float bossHp = boss.GetCurHp; // boss.GetCurHp�� ȣ���Ͽ� ���� ü���� �����ɴϴ�.

            // uiHp�� 0���� 1 ������ ���� �Ҵ��մϴ�.
            uiHp = Mathf.Clamp01(bossHp / 100); // ���� ���, boss.MaxHp�� �ִ� ü�� ���� ���Դϴ�.

            // hpBar.fillAmount�� uiHp ���� �����մϴ�.
            if (hpBar != null)
            {
                hpBar.fillAmount = uiHp;
            }
        }

        // ī�޶� �ٶ󺸵��� UI�� �����մϴ�.
        if (Camera.main != null)
        {
            rectTr.LookAt(Camera.main.transform);
        }
    }

    private float uiHp = 0f; 

}

