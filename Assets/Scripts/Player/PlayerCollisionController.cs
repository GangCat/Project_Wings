using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public delegate void ChangeCollisionConditionDelegate(Collision _coli, bool _bool);
    private ChangeCollisionConditionDelegate collisionCallback = null;

    public void Init(ChangeCollisionConditionDelegate _Callback)
    {
        collisionCallback = _Callback;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Ãæµ¹");
        collisionCallback?.Invoke(collision, true);
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionCallback?.Invoke(collision,false);
    }


}
