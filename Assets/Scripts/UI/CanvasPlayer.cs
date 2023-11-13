using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPlayer : MonoBehaviour
{
    public void Init()
    {
        hpPanel = GetComponentInChildren<PlayerHpCountPanel>();
        spPanel = GetComponentInChildren<PlayerStaminaPanel>();
        hpPanel.Init();
        spPanel.Init();
    }

    private PlayerHpCountPanel hpPanel;
    private PlayerStaminaPanel spPanel;
}
