using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCamera : MonoBehaviour
{
    public void Init(VoidVoidDelegate _actionFinishCallback)
    {
        anim = GetComponent<Animator>();
        cam = GetComponent<Camera>();
        actionFinishCallback = _actionFinishCallback;
        cam.enabled = false;
    }

    public void StartAction(int _curPhaseNum)
    {
        // Å×½ºÆ®
        if (_curPhaseNum < 1)
        {
            cam.enabled = true;
            anim.SetTrigger($"Phase{_curPhaseNum}");
        }
        else
            Invoke("FinishAction",1f);
    }

    private void FinishAction()
    {
        actionFinishCallback?.Invoke();
        cam.enabled = false;
    }

    private Animator anim = null;
    private VoidVoidDelegate actionFinishCallback = null;
    private Camera cam = null;
}
