using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Init()
    {
        canvasBoss.Init();
    }


    [SerializeField]
    private CanvasBoss canvasBoss = null;
}
