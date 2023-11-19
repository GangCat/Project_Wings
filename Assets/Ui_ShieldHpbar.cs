using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_ShieldHpbar : MonoBehaviour
{
    [SerializeField]
    private Image hpBar = null;
    [SerializeField]
    private RectTransform uiTransform = null;
    [SerializeField]
    private float initialSpeed = 50.0f;
    [SerializeField]
    private float acceleration = 0.5f;
    [SerializeField]
    private Vector3 targetPosition = Vector3.zero;
    [SerializeField]
    private BossShieldGenerator boss = null;
    private RectTransform rectTr = null;

    void Start()
    {
        rectTr = GetComponent<RectTransform>();
        uiHp = boss.GetCurHp;
        curHp = boss.GetCurHp;
        StartCoroutine(MoveUI());
    }
    private void Update()
    {
        if (boss != null)
        {
            float bossHp = boss.GetCurHp;

            if(curHp != bossHp) 
            {
                curHp = bossHp;
                uiHp = Mathf.Clamp01(bossHp / 100);
                StartCoroutine(MoveUI());
                if (hpBar != null)
                {

                hpBar.fillAmount = uiHp;
                }
            }
        }

        //// 카메라를 바라보도록 UI를 조정합니다.
        //if (Camera.main != null)
        //{
        //    rectTr.LookAt(Camera.main.transform);
        //}
    }

    private IEnumerator MoveUI()
    {
        float currentSpeed = initialSpeed;
        Vector3 beforePos = uiTransform.localPosition;
        Debug.Log(beforePos);

        while (Vector3.Distance(uiTransform.localPosition, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - uiTransform.localPosition).normalized;
            currentSpeed += acceleration * Time.deltaTime;
            uiTransform.localPosition += direction * currentSpeed * Time.deltaTime;

            yield return null; 
        }
        uiTransform.localPosition = targetPosition;
        yield return new WaitForSeconds(3f);

        currentSpeed = initialSpeed;
        while (Vector3.Distance(uiTransform.localPosition, beforePos) > 0.1f)
        {
            Vector3 direction = (beforePos - uiTransform.localPosition).normalized;
            currentSpeed += acceleration * Time.deltaTime;
            uiTransform.localPosition += direction * currentSpeed * Time.deltaTime;

            yield return null;
        }

    }

    private float uiHp = 0f;
    private float curHp = 0f;
}

