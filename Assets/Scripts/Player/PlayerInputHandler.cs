using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private float inputX;
    private float inputZ;

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxis("Vertical");
    }

    public float GetInputX()
    {
        return inputX;
    }

    public float GetInputZ()
    {
        return inputZ;
    }
}
