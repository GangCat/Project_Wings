using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Init(VoidVoidDelegate _resumeCallback)
    {
        canvasBoss = GetComponentInChildren<CanvasBoss>();
        canvasPlayer = GetComponentInChildren<CanvasPlayer>();
        canvasUI = GetComponentInChildren<CanvasUI>();
        canvasAlert = GetComponentInChildren<CanvasAlertMessage>();
        canvasPause = GetComponentInChildren<CanvasPauseMenu>();
        canvasBoss.Init();
        canvasPlayer.Init();
        canvasUI.Init();
        canvasAlert.Init();
        canvasPause.Init(_resumeCallback);
    }

    public void BossHpUpdate(float _ratio)
    {
        canvasBoss.UpdateHpBar(_ratio);
    }

    public void BossShieldUpdate(float _ratio)
    {
        canvasBoss.UpdateShieldBar(_ratio);
    }

    public void RemoveBossShield()
    {
        canvasBoss.RemoveBossShield();
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
    public void SetPauseManger(bool _bool)
    {
        canvasPause.SetPauseMenu(_bool);
    }

    private CanvasBoss canvasBoss = null;
    private CanvasPlayer canvasPlayer = null;
    private CanvasUI canvasUI = null;
    private CanvasAlertMessage canvasAlert = null;
    private CanvasPauseMenu canvasPause = null;
}
