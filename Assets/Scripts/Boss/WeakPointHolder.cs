using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPointHolder : MonoBehaviour
{
    public void Init()
    {
        arrWeakPoints = GetComponentsInChildren<WeakPoint>();
    }

    public WeakPoint[] WeakPoints => arrWeakPoints;


    private WeakPoint[] arrWeakPoints = null;
}
