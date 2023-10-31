using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private float inputX;
    private float inputZ;
    private float inputMouseX;
    private float inputMouseY;


    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");
        inputMouseX = Input.GetAxis("Mouse X");
        inputMouseY = Input.GetAxis("Mouse Y");
    }

    public float InputX => inputX;
    public float InputZ => inputZ;
    public float InputMouseX => inputMouseX;
    public float InputMouseY => inputMouseY;

}
