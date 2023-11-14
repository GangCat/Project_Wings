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
            //�Ͻ����� �޴�â ���� �ڵ�ִ� �ڸ�
        }
        else
        {
            ResumeGame();
            //�޴�ò ���� �ڵ� �ִ� �ڸ�
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
