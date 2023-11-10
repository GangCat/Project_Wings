using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBoss : MonoBehaviour
{
    public void Init()
    {
        hpbar.Init();
    }

    public void UpdateHpBar(float _ratio)
    {
        hpbar.UpdateLength(_ratio);
    }

    [SerializeField]
    private ImageProgressbar hpbar = null;
}
