using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : AnimationControllerBase
{
    private Animator animCtrl = null;


    private void Start()
    {
        animCtrl = GetComponent<Animator>();
    }


    public void SetAnimBool(string _name, bool _bool)
    {
        animCtrl.SetBool(_name, _bool);
    }
}
