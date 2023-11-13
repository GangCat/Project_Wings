using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPatternController : MonoBehaviour
{ 
    public void Init(VoidVoidDelegate _patternFinishCallback)
    {
        lastPatternCollider = GetComponentInChildren<LastPatternCollider>();
        lastPatternCollider.Init(_patternFinishCallback);
    }

    public void StartPattern()
    {
        lastPatternCollider.Enable(true);
    }



    private LastPatternCollider lastPatternCollider = null;
}
