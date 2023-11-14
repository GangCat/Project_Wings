using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Init()
    {
        canvasBoss = GetComponentInChildren<CanvasBoss>();
        canvasPlayer = GetComponentInChildren<CanvasPlayer>();
        canvasUI = GetComponentInChildren<CanvasUI>();
        canvasBoss.Init();
        canvasPlayer.Init();
        canvasUI.Init();
    }

    public void BossHpUpdate(float _ratio)
    {
        canvasBoss.UpdateHpBar(_ratio);
    }

    public void BossShieldUpdate(float _ratio)
    {
        canvasBoss.UpdateShieldBar(_ratio);
    }
    public void UpdateSp(int _Stamina)
    {
        canvasPlayer.UpdateSp(_Stamina);
    }

    public void UpdateHp(float _curHpRatio)
    {
        canvasPlayer.UpdateHp(_curHpRatio);
    }

    public void GameClear()
    {
        Debug.Log("GameClear");
        canvasUI.GameClear();
        //canvasInformation.GameClear();
    }

    private CanvasBoss canvasBoss = null;
    private CanvasPlayer canvasPlayer = null;
    private CanvasUI canvasUI = null;
}
