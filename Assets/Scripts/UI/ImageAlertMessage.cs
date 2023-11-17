using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageAlertMessage : MonoBehaviour
{
    public void Init()
    {
        textAlert = GetComponentInChildren<TextAlertMessage>();
        textAlert.Init();
    }

    public void AlertDanger()
    {
        //경고 사운드 재생
        textAlert.AlertDanger(alertMessage);
    }


    [SerializeField]
    [TextArea]
    private string alertMessage = "";

    private TextAlertMessage textAlert = null;
}
