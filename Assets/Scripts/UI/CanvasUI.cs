using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void GameClear()
    {
        gameObject.SetActive(true);
    }

    [SerializeField]
    private Image image = null;
}
