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
    [SerializeField]
    private VirtualMouse vm;
    [SerializeField]
    private CameraMovement cameraMove;

    public void Init(VoidVoidDelegate _resumeCallback)
    {
        ResumeCallback = _resumeCallback;
        pauseMenu = GetComponentInChildren<Image>().gameObject;
        sensitiveSlider.value = vm.sensitive;
        freeLookSensitiveSlider.value = cameraMove.freeLockSensitive * 0.5f;
        //volumeSlider.onValueChanged.AddListener(delegate { ChangeVolumeCallback(volumeSlider.value); });
        sensitiveSlider.onValueChanged.AddListener(delegate { ChangeSensitive(); });
        freeLookSensitiveSlider.onValueChanged.AddListener(delegate { ChangeFreeLookSensitive(); });
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
        ResumeCallback();
        SceneManager.LoadScene("CampusScene");
    }

    public void ChangeSensitive()
    {
        vm.sensitive = sensitiveSlider.value;
    }

    private void ChangeFreeLookSensitive()
    {
        cameraMove.freeLockSensitive = sensitiveSlider.value * 50f;
    }

}
