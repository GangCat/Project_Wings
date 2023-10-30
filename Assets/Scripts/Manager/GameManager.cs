using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        audioMng = FindFirstObjectByType<AudioManager>();
    }

    private void Start()
    {
        InitManagers();
    }

    private void InitManagers()
    {
        audioMng.Init();
    }

    private AudioManager audioMng = null;
}
