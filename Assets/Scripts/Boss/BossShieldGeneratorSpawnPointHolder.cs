using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldGeneratorSpawnPointHolder : MonoBehaviour
{
    public void Init()
    {
        //arrShieldGeneratorSpawnPoints = GetComponentsInChildren<BossShieldGeneratorSpawnPoint>();
    }

    public BossShieldGeneratorSpawnPoint[] ShieldGeneratorSpawnPoints => arrShieldGeneratorSpawnPoints;

    [SerializeField]
    private BossShieldGeneratorSpawnPoint[] arrShieldGeneratorSpawnPoints = null;
}
