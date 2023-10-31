using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseChecker : MonoBehaviour
{
    public Image virtualCursor;
    
    public float decreaseSpeed = 1f;
    public float sensitive = 1f;

    private float inputMouseX;
    private float inputMouseY;

    private float inputPosX;
    private float inputPosY;
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

        inputPosX += inputMouseX * sensitive;
        inputPosY += inputMouseY * sensitive;

        //inputPosX = Mathf.Clamp(inputPosX, -150f, 150f);
        //inputPosY = Mathf.Clamp(inputPosY, -150f, 150f);

        float radius = 150f; // ������

        // �Է� ��ġ�� ���� ��� ���� ����
        float distanceFromOrigin = Mathf.Sqrt(inputPosX * inputPosX + inputPosY * inputPosY);

        if (distanceFromOrigin > radius)
        {
            // �Է� ��ġ�� ���������� �ۿ� ������, ��ġ�� ���� ���� �̵�
            float angle = Mathf.Atan2(inputPosY, inputPosX);
            inputPosX = radius * Mathf.Cos(angle);
            inputPosY = radius * Mathf.Sin(angle);
        }

        Vector2 inputPos = new Vector2(inputPosX, inputPosY);
        float maxRadius = 100f;

        if (inputPos.magnitude < maxRadius)
        {
            inputPosX = Mathf.MoveTowards(inputPosX, 0, decreaseSpeed * Time.deltaTime);
            inputPosY = Mathf.MoveTowards(inputPosY, 0, decreaseSpeed * Time.deltaTime);
        }

        //if (Mathf.Abs(inputPosX) + Mathf.Abs(inputPosY) <= 150f)
        //{
        //    inputPosX = Mathf.MoveTowards(inputPosX, 0, decreaseSpeed * Time.deltaTime);
        //    inputPosY = Mathf.MoveTowards(inputPosY, 0, decreaseSpeed * Time.deltaTime);
        //}


        virtualCursor.transform.position = new Vector3(centerPosition.x + inputPosX, centerPosition.y + inputPosY);
        // x�� y ��ǥ�� �и��մϴ�


        // ����� ����մϴ�
       // Debug.Log("Mouse X: " + mouseX + ", Mouse Y: " + mouseY);
        Debug.Log("input X: " + inputPosX + ", input Y: " + inputPosY);

    }
    
}
