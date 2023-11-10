using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour, IDamageable
{
    public void Init()
    {
        mr = GetComponent<MeshRenderer>();
        oriLayer = gameObject.layer;
        //brokenLayer = LayerMask.NameToLayer("BossShieldBroken");
        curGeneratorCount = 4;
        UpdateEffect();
    }

    public bool IsShieldBreak()
    {
        return gameObject.layer.Equals(brokenLayer);
    }

    public void RespawnGenerator()
    {
        SetActive(true);
        curGeneratorCount = 4;
        UpdateEffect();
    }

    public void SetActive(bool _active)
    {
        gameObject.SetActive(_active);
    }

    public void ChangeLayerToggle()
    {
        gameObject.layer = gameObject.layer.Equals(oriLayer) ? brokenLayer : oriLayer;
    }

    public void GeneratorDestroy()
    {
        --curGeneratorCount;
        UpdateEffect();
    }

    private void UpdateEffect()
    {
        if (curGeneratorCount < 1)
            SetActive(false);
        mr.material.SetFloat("_Dissolve", curGeneratorCount * 0.25f);
        // ½¦ÀÌ´õ¿¡¼­ ¼öÄ¡ 1, 0.75, 0.5, 0.25 Á¶Àý
        // curGeneratorCount * 0.25f;
        // ¹æ¾î¸· ³óµµ ¿¶¾îÁü.
    }

    public float GetCurHp => 99999;

    public void GetDamage(float _dmg)
    {
        Debug.Log($"Shield Hit!");
    }

    private int curGeneratorCount = 0;
    private MeshRenderer mr = null;
    private LayerMask brokenLayer;
    private LayerMask oriLayer;
}
