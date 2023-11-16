using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private VoidBoolDelegate OnPauseCallback;
    public void Init(VoidBoolDelegate _OnPauseDelegate)
    {
        OnPauseCallback = _OnPauseDelegate;
        OnPauseCallback?.Invoke(false);
    }

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
            OnPauseCallback?.Invoke(true);
        }
        else
        {
            ResumeGame();
            OnPauseCallback?.Invoke(false);
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
