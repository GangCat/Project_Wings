using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public Transform tr;
    public PlayerInputHandler input;

    [Header("Status Data")]
    public int maxHealth = 100;
    public int stamina = 100;

    [Header("Move Data")]
    public float moveBackVelocityLimit = 0f;
    public float moveForwardVelocityLimit = 0f;
    public float moveAccel = 0f;

    [Header("Cam Data")]
    public float rotCamXAxisSpeed = 0f;
    public float rotCamYAxisSpeed = 0f;
    public float minAngleX = 0f;
    public float maxAngleX = 0f;

    [Header("Roll Data")]
    public float rollAccel = 0f;
    public float rollMaxVelocity = 0f;
    public float rollMaxAngle = 0f;

}
