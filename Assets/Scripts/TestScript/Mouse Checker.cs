using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseChecker : MonoBehaviour
{
    public Image virtualCursor;
    
    public float decreaseSpeed = 100f;
    public float sensitive = 2f;

    private float inputMouseX;
    private float inputMouseY;

    public float inputPosX;
    public float inputPosY;
    Vector2 centerPosition;

    private void Start()
    {
        centerPosition = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        inputPosX = 0f;
        inputPosY = 0f;
    }

    private void Update()
    {
        inputMouseX = Input.GetAxis("Mouse X");
        inputMouseY = Input.GetAxis("Mouse Y");
        Mathf.Clamp(inputMouseX, -3, 3);
        Mathf.Clamp(inputMouseY, -3, 3);

        inputPosX += inputMouseX * sensitive;
        inputPosY += inputMouseY * sensitive;


        float radius = 100f;

        float distanceFromOrigin = Mathf.Sqrt(inputPosX * inputPosX + inputPosY * inputPosY);

        if (distanceFromOrigin > radius)
        {
            float angle = Mathf.Atan2(inputPosY, inputPosX);
            inputPosX = radius * Mathf.Cos(angle);
            inputPosY = radius * Mathf.Sin(angle);
        }

        Vector2 inputPos = new Vector2(inputPosX, inputPosY);

        float maxRadius = 90f;

        if (inputPos.magnitude < maxRadius)
        {
            inputPosX = Mathf.MoveTowards(inputPosX, 0, decreaseSpeed * Time.deltaTime);
            inputPosY = Mathf.MoveTowards(inputPosY, 0, decreaseSpeed * Time.deltaTime);
        }

        virtualCursor.transform.position = new Vector3(centerPosition.x + inputPosX, centerPosition.y + inputPosY);

        Debug.Log("input X: " + inputPosX + ", input Y: " + inputPosY);

    }
    
}
