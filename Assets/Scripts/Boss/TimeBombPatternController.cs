using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBombPatternController : MonoBehaviour
{
    public void Init(VoidVoidDelegate _patternFinishDelegate)
    {
        patternFinishCallback = _patternFinishDelegate;
    }

    public void StartPattern()
    {
        Debug.Log("PatterStart");
        Invoke("FinishPattern", 5f);
    }

    private void FinishPattern()
    {
        Debug.Log("PatterFinish");
        patternFinishCallback?.Invoke();
    }



    [SerializeField]
    private GameObject timeBombPrefab = null;
    [SerializeField]
    private Transform[] timeBombDestTr = null;

    private VoidVoidDelegate patternFinishCallback = null;
}
