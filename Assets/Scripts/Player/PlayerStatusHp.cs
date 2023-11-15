using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        volumeProfile.TryGet(out colorAd);
        colorAd.saturation.value = 30f;
    }

    public float GetCurHp => curHp;

    public void GetDamage(float _dmg)
    {
        if (gameObject.layer.Equals(LayerMask.NameToLayer("PlayerInvincible")))
            return;

        curHp -= _dmg;

        CameraShake.Instance.ShakeCamera(1f, 3f);

        StartCoroutine("SaturationCoroutine");

        hpUpdateCallback?.Invoke(curHp / maxHp);

        if (curHp <= 0)
            deadCallback?.Invoke();

    }

    private IEnumerator SaturationCoroutine()
    {
        float startTime = Time.time;
        float elapsedTimeRatio = 0f;

        while (elapsedTimeRatio < 1)
        {
            elapsedTimeRatio = (Time.time - startTime) / saturationTime;

            colorAd.saturation.value = -80f + (elapsedTimeRatio * 110f);

            yield return null;
        }
        colorAd.saturation.value = 30f;
    }

    public void MaxHeal()
    {
        curHp = maxHp;
    }

    private VoidVoidDelegate deadCallback = null;
    private VoidFloatDelegate hpUpdateCallback = null;
    private VolumeProfile volumeProfile = null;
    private ColorAdjustments colorAd;
    [SerializeField]
    private float saturationTime = 2f;
}
