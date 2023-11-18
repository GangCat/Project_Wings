using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageAlertMessage : MonoBehaviour
{
    public void Init()
    {
        soundManager = SoundManager.Instance;
        soundManager.Init(gameObject);
        textAlert = GetComponentInChildren<TextAlertMessage>();
        textAlert.Init();
    }

    public void AlertDanger()
    {
        //경고 사운드 재생
        soundManager.PlayAudio(GetComponent<AudioSource>(),(int)SoundManager.ESounds.SCREENWARNINGSOUND);
        textAlert.AlertDanger(alertMessage[0]);
    }

    public void FirstPatternAlert()
    {
        textAlert.AlertDanger(alertMessage[1]);
    }


    [SerializeField]
    [TextArea]
    private string[] alertMessage = null;

    private TextAlertMessage textAlert = null;

    private SoundManager soundManager = null;
}
