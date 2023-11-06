using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpCountPanel : MonoBehaviour
{
    public void Init()
    {
        arrImageHp = GetComponentsInChildren<Image>();
        curHp = 6;
    }

    public void DecreaseHp()
    {
        arrImageHp[curHp - 1].sprite = null;
        arrImageHp[curHp - 1].color = new Color(0f, 0f, 0f, 0f);
    }

    private Image[] arrImageHp = null;
    private int curHp = 0;
}
