using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 플레이어 체력은 피격횟수 기준
/// </summary>
public class PlayerStatusHp : StatusHp, IPlayerDamageable
{
    public void Init(VoidVoidDelegate _deadCallback, VoidFloatDelegate _hpUpdateCallback, VolumeProfile _globalVolume)
    {
        curHp = maxHp;
        deadCallback = _deadCallback;
        hpUpdateCallback = _hpUpdateCallback;
        volumeProfile = _globalVolume;
    }

    public float GetCurHp => curHp;

    public void GetDamage(float _dmg)
    {
        if (gameObject.layer.Equals(LayerMask.NameToLayer("PlayerInvincible")))
            return;

        curHp -= _dmg;

        StartCoroutine("SaturationCoroutine");

        hpUpdateCallback?.Invoke(curHp / maxHp);

        if (curHp <= 0)
            deadCallback?.Invoke();

    }

    private IEnumerator SaturationCoroutine()
    {
        float startTime = Time.time;
        float elapsedTimeRatio = 0f;
        ColorAdjustments colorAd;
        volumeProfile.TryGet(out colorAd);

        while (elapsedTimeRatio < 1)
        {
            elapsedTimeRatio = (Time.time - startTime) / saturationTime;

            colorAd.saturation.value = -80f + (elapsedTimeRatio * 130f);

            yield return null;
        }
        colorAd.saturation.value = 50f;
    }

    public void MaxHeal()
    {
        curHp = maxHp;
    }

    private VoidVoidDelegate deadCallback = null;
    private VoidFloatDelegate hpUpdateCallback = null;
    private VolumeProfile volumeProfile = null;
    [SerializeField]
    private float saturationTime = 2f;
}
