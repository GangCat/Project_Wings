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

        SetStartPosAndRot(_curPhaseNum);

        anim.SetTrigger($"Phase{_curPhaseNum}");
        StartCoroutine(CheckIsAnimFinishCoroutine(_curPhaseNum));

        if (_curPhaseNum > 2)
            isLastAction = true;

        Debug.Log(_curPhaseNum);
    }

    private void SetStartPosAndRot(int _curPhaseNum)
    {
        if(_curPhaseNum == 0)
        {
            SetPos(230, 539, -405);
            SetRot(10.902f, -80, 0f);
        }
        else if(_curPhaseNum == 1)
        {
            SetPos(-594, 190, -862);
            SetRot(10.902f, 34.582f, 0);
        }
        else if(_curPhaseNum == 2)
        {
            SetPos(-4.3f, 122f, -162.6f);
            SetRot(-45f, 0f, 0f);
        }
        else if(_curPhaseNum >= 3)
        {
            SetPos(0f, 597f, -972f);
            SetRot(14.354f, 0f, 0f);
        }
    }

    private void SetPos(float _x, float _y, float _z)
    {
        transform.position = new Vector3(_x, _y, _z);
    }

    private void SetRot(float _x, float _y, float _z)
    {
        transform.rotation = Quaternion.Euler(new Vector3(_x, _y, _z));
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
