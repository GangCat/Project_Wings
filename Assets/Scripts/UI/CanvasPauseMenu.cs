using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasPauseMenu : MonoBehaviour
{
    private GameObject pauseMenu;
    private VoidVoidDelegate ResumeCallback;
    private VoidFloatDelegate ChangeVolumeCallback;
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private Slider sensitiveSlider;
    [SerializeField]
    private Slider freeLookSensitiveSlider;

    public void Init(VoidVoidDelegate _resumeCallback)
    {
        ResumeCallback = _resumeCallback;
        pauseMenu = GetComponentInChildren<Image>().gameObject;
        //volumeSlider.onValueChanged.AddListener(delegate { ChangeVolumeCallback(volumeSlider.value); });
        //sensitiveSlider.onValueChanged.AddListener(delegate { });
    }

    public void SetPauseMenu(bool _bool)
    {
        pauseMenu.SetActive(_bool);
    }

    public void ResumeGame()
    {
        ResumeCallback();
    }

    public void RetryGame()
    {
        ResumeCallback();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ExitGame()
    {
        // 캠퍼스로 돌아가기
    }


}
