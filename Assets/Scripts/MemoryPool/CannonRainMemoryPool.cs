using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRainMemoryPool : MonoBehaviour
{
    public void Init()
    {
        memoryPool = new MemoryPool(cannonRainBallPrefab, 100, transform);
    }

    public GameObject ActivateCannonBall()
    {
        GameObject cannonRainBallGo = memoryPool.ActivatePoolItem(5, transform);
        return cannonRainBallGo;
    }

    public void DeactivateCannonBall(GameObject _deactivateGo)
    {
        memoryPool.DeactivatePoolItem(_deactivateGo);
    }
    private MemoryPool memoryPool;
    [SerializeField]
    private GameObject cannonRainBallPrefab = null;
}
