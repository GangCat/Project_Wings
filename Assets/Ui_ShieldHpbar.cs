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
            float bossHp = boss.GetCurHp; // boss.GetCurHp를 호출하여 현재 체력을 가져옵니다.

            // uiHp에 0에서 1 사이의 값을 할당합니다.
            uiHp = Mathf.Clamp01(bossHp / 100); // 예를 들어, boss.MaxHp는 최대 체력 값일 것입니다.

            // hpBar.fillAmount에 uiHp 값을 적용합니다.
            if (hpBar != null)
            {
                hpBar.fillAmount = uiHp;
            }
        }

        // 카메라를 바라보도록 UI를 조정합니다.
        if (Camera.main != null)
        {
            rectTr.LookAt(Camera.main.transform);
        }
    }

    private float uiHp = 0f; 

}

