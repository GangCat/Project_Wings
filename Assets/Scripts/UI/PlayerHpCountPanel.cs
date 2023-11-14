using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpCountPanel : MonoBehaviour
{
    public void Init()
    {
        imageProgress = GetComponentInChildren<ImageProgressbar>();
        imageProgress.Init();
    }

    public void UpdateHp(float _curHpRatio)
    {
        imageProgress.UpdateLength(_curHpRatio);
    }

    private ImageProgressbar imageProgress = null;
}
