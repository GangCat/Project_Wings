using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPauseMenu : MonoBehaviour
{
    private GameObject pauseMenu;


    public void Init()
    {
        pauseMenu = GetComponentInChildren<Image>().gameObject;
    }

    public void SetPauseMenu(bool _bool)
    {
        pauseMenu.SetActive(_bool);
    }

}
