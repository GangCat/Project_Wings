using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private int inputX;
    private int inputZ;

    void Update()
    {
        inputX = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        inputZ = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
    }

    public int GetInputX()
    {
        return inputX;
    }

    public int GetInputZ()
    {
        return inputZ;
    }
}
