using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCamera : MonoBehaviour
{
    public void Init(VoidBoolDelegate _actionFinishCallback)
    {
        anim = GetComponent<Animator>();
        cam = GetComponent<Camera>();
        actionFinishCallback = _actionFinishCallback;
        cam.enabled = false;
    }

    public void StartAction(int _curPhaseNum)
    {
        cam.enabled = true;
        anim.SetTrigger($"Phase{_curPhaseNum}");
        StartCoroutine(CheckIsAnimFinishCoroutine(_curPhaseNum));

        if (_curPhaseNum > 2)
            isLastAction = true;

        Debug.Log(_curPhaseNum);
    }

    private IEnumerator CheckIsAnimFinishCoroutine(int _curPhaseNum)
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName($"Phase{_curPhaseNum}"))
            {
                FinishAction();
                yield break;
            }

            yield return null;
        }
    }

    private void FinishAction()
    {
        actionFinishCallback?.Invoke(isLastAction);
        cam.enabled = false;
    }

    private Animator anim = null;
    private VoidBoolDelegate actionFinishCallback = null;
    private Camera cam = null;

    private bool isLastAction = false;
}
