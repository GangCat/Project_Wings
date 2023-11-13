using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Init()
    {
        canvasBoss = GetComponentInChildren<CanvasBoss>();
        canvasPlayer = GetComponentInChildren<CanvasPlayer>();
        canvasBoss.Init();
        canvasPlayer.Init();
    }

    public void BossHpUpdate(float _ratio)
    {
        canvasBoss.UpdateHpBar(_ratio);
    }
    public void PlayerSpUpdate(int _Stamina)
    {
        canvasPlayer.UpdateSp(_Stamina);
    }

    private CanvasBoss canvasBoss = null;
    private CanvasPlayer canvasPlayer = null;
}
