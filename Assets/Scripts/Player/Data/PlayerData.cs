using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public Rigidbody rb;

    [Header("Status Data")]
    public int maxHealth = 100;
    public int stamina = 100;

    [Header("Move Data")]
    public float maxSpeed = 10;
    public float accle = 5;


}
