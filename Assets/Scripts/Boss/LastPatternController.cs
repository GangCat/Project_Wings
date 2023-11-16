using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPatternController : MonoBehaviour
{ 
    public void Init(VoidVoidDelegate _patternFinishCallback)
    {
        lastPatternCollider.Init(_patternFinishCallback);
    }

    public void StartPattern()
    {
        lastPatternCollider.Enable(true);
    }


    [SerializeField]
    private LastPatternCollider lastPatternCollider = null;
}
