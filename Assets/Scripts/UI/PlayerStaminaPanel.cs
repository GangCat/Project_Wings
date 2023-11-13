using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaPanel : MonoBehaviour
{
    public void Init()
    {
        arrImageStamina = GetComponentsInChildren<Image>();
        curStamina = 3;
    }

    public void DecreaseHp()
    {
        arrImageStamina[curStamina - 1].sprite = null;
        arrImageStamina[curStamina - 1].color = new Color(0f, 0f, 0f, 0f);
    }

    [SerializeField]
    private Image[] arrImageStamina = null;
    private int curStamina = 0;
}
