using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour
{
    public void Init()
    {
        bossBoxCollider = GetComponent<BoxCollider>();
    }

    public BoxCollider BossBoxCol => bossBoxCollider;

    public void SetTag(string _tagName)
    {
        gameObject.tag = _tagName;
    }

    public void ResetAll()
    {
        gameObject.tag = "Untagged";
        bossBoxCollider.center = Vector3.zero;
        bossBoxCollider.size = Vector3.one;
        bossBoxCollider.enabled = false;
    }

    public void SetPos(Vector3 _pos)
    {
        bossBoxCollider.center = _pos;
    }

    public void SetSize(Vector3 _size)
    {
        bossBoxCollider.size = _size;
    }

    public void SetEnableCollider()
    {
        bossBoxCollider.enabled = true;
    }

    private BoxCollider bossBoxCollider = null;
}
