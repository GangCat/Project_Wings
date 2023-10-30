using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float maxSpeed = 10f;
    private float accle = 2f;

    private Vector3 velocity;

    private void FixedUpdate()
    {
        transform.Translate(velocity * Time.deltaTime);
    }

    public void VelocityUpdate(int _dir)
    {
        velocity += transform.forward * accle * Time.deltaTime * _dir;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }



}
