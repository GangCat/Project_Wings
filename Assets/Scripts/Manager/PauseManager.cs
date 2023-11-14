using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
            //일시정지 메뉴창 띄우는 코드넣는 자리
        }
        else
        {
            ResumeGame();
            //메뉴챵 끄는 코드 넣는 자리
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void ForcePause()
    {
        isPaused = true;
        PauseGame();
    }

    public void ForceResume()
    {
        isPaused = false;
        ResumeGame();
    }
}
