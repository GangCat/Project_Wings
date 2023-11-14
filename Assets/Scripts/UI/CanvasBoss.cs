using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBoss : MonoBehaviour
{
    public void Init()
    {
        hpBar.Init();
        shieldBar.Init();
    }

    public void UpdateHpBar(float _ratio)
    {
        hpBar.UpdateLength(_ratio);
    }

    public void UpdateShieldBar(float _ratio)
    {
        shieldBar.UpdateLength(_ratio);
    }

    [SerializeField]
    private ImageProgressbar hpBar = null;
    [SerializeField]
    private ImageProgressbar shieldBar = null;
}
