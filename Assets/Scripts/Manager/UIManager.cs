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

    private CanvasBoss canvasBoss = null;
    private CanvasPlayer canvasPlayer = null;
}
