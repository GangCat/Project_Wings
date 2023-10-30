using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBoss : MonoBehaviour
{
    public void Init()
    {
        hpbar.Init();
    }

    [SerializeField]
    private ImageProgressbar hpbar = null;
}
