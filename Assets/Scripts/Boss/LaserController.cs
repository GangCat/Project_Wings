using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public delegate void DestroyBombDelegate(GameObject _bombGo);
    public void Init(float _launchDuration, float _lengthPerSec, DestroyBombDelegate _destroyBombCallback)
    {
        destroyBombCallback = _destroyBombCallback;
        launchDuration = _launchDuration;
        lengthPerSec = _lengthPerSec;
        waitFixedTime = new WaitForFixedUpdate();
        StartCoroutine(LaunchLaserCoroutine(Time.time));
    }

    private IEnumerator LaunchLaserCoroutine(float _startTime)
    {
        while(Time.time - _startTime < launchDuration)
        {
            //사운드
            // 연출
            transform.localScale += Vector3.forward * lengthPerSec * Time.fixedDeltaTime;

            yield return waitFixedTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TimeBomb"))
            destroyBombCallback?.Invoke(other.gameObject);
    }

    private DestroyBombDelegate destroyBombCallback = null;
    private float launchDuration = 0f;
    private float lengthPerSec = 0f;
    private WaitForFixedUpdate waitFixedTime = null;
}
