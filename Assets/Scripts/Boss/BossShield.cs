using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour, IDamageable
{
    public void Init(VoidVoidDelegate _restorShieldFinishCallback)
    {
        mr = GetComponent<MeshRenderer>();
        mc = GetComponent<MeshCollider>();
        //brokenLayer = LayerMask.NameToLayer("BossShieldBroken");
        curGeneratorCount = 4;
        restorShieldFinishCallback = _restorShieldFinishCallback;
        UpdateEffect();
        mc.enabled = false;
    }

    public void RespawnGenerator()
    {
        mc.enabled = true;
        curGeneratorCount = 4;
        UpdateEffect();
    }

    public void GeneratorDestroy()
    {
        --curGeneratorCount;
        UpdateEffect();
    }

    private void UpdateEffect()
    {
        mr.material.SetFloat("_Dissolve", curGeneratorCount * 0.25f);

        if (curGeneratorCount < 1)
        {
            mc.enabled = false;
            StartCoroutine("RestoreShieldCoroutine");
        }
        // ½¦ÀÌ´õ¿¡¼­ ¼öÄ¡ 1, 0.75, 0.5, 0.25 Á¶Àý
        // curGeneratorCount * 0.25f;
        // ¹æ¾î¸· ³óµµ ¿¶¾îÁü.
    }

    private IEnumerator RestoreShieldCoroutine()
    {
        float startTime = Time.time;
        float remainTimeRatio = 0f;
        while (remainTimeRatio <= 1)
        {
            mr.material.SetFloat("_Dissolve", remainTimeRatio);

            remainTimeRatio = (Time.time - startTime) / restoreShieldDelay;
            yield return new WaitForFixedUpdate();
        }

        restorShieldFinishCallback?.Invoke();
        RespawnGenerator();
    }

    public float GetCurHp => 99999;

    public void GetDamage(float _dmg)
    {
        Debug.Log($"Shield Hit!");
    }

    private int curGeneratorCount = 0;
    private MeshRenderer mr = null;
    private MeshCollider mc = null;
    private float restoreShieldDelay = 30f;
    private VoidVoidDelegate restorShieldFinishCallback = null;
}
