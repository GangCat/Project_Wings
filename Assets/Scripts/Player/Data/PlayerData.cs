using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public Rigidbody rb;
    public Transform tr;
    public PlayerInputHandler inputHandler;

    [Header("Status Data")]
    public int maxHealth = 100;
    public int stamina = 100;

    [Header("Move Data")]
    public float maxSpeed = 10;
    public float accle = 5;
    public bool isDash = false;
    public bool isDodge = false;

    public float maxRotSpeed = 10;
    public float rotAccle = 10;

}
