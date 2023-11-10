using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour, IDamageable
{
    public void Init()
    {
        oriLayer = gameObject.layer;
        destroyLayer = LayerMask.NameToLayer("BossShieldDestroyed");
        RespawnGenerator();
        UpdateEffect();
    }

    public void RespawnGenerator()
    {
        curGeneratorCount = 4;
    }

    public void SetActive(bool _active)
    {
        gameObject.SetActive(_active);
    }

    public void ChangeLayerToggle()
    {
        gameObject.layer = gameObject.layer.Equals(oriLayer) ? destroyLayer : oriLayer;
    }

    public void GeneratorDestroy()
    {
        --curGeneratorCount;
        UpdateEffect();
    }

    private void UpdateEffect()
    {
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
    private LayerMask destroyLayer;
    private LayerMask oriLayer;
}
