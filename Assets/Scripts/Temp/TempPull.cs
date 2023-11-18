using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPull : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 거리가 10 이하이면
        // 위로 주는 속도 추가
        StartCoroutine(SetVelocity(other));
        
    }

    private void Update()
    {
        transform.localScale += Vector3.one * 5f * Time.deltaTime;
    }


    private IEnumerator SetVelocity(Collider _other)
    {
        pullForce = curve.Evaluate((Time.time * 0.1f) % curve.length);

        Vector3 forceDir = transform.position - _other.transform.position;

        _other.GetComponent<Rigidbody>().AddForce(forceDir.normalized * pullForce * pullForceAmount);


        yield return new WaitForSeconds(0.2f);

        if (!_other.gameObject.activeSelf)
            yield break;

        if(Vector3.SqrMagnitude(transform.position - _other.transform.position) < Mathf.Pow(3f, 2f))
            StartCoroutine(SetVelocityUp(_other));
        else
            StartCoroutine(SetVelocity(_other));
    }

    private IEnumerator SetVelocityUp(Collider _other)
    {
        _other.GetComponent<Rigidbody>().AddForce(Vector3.up * upForce);
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(SetVelocityUp(_other));
    }


    public AnimationCurve curve;
    float pullForce;

    public float pullForceAmount = 200f;
    public float upForce = 2000f;
    public float rightForce = 100f;
    public SphereCollider box;
}
