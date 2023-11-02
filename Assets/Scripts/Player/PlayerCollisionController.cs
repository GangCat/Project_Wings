using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public void Init()
    {

    }


    //bool CheckHitWall(Vector3 movement)
    //{

    //    movement = transform.TransformDirection(movement);

    //    float scope = 1f;

    //    List<Vector3> rayPositions = new List<Vector3>();
    //    rayPositions.Add(transform.position + Vector3.up * 0.1f);
    //    rayPositions.Add(transform.position + Vector3.up * boxCollider.size.y * 0.5f);
    //    rayPositions.Add(transform.position + Vector3.up * boxCollider.size.y);


    //    foreach (Vector3 pos in rayPositions)
    //    {
    //        Debug.DrawRay(pos, movement * scope, Color.red);
    //    }


    //    foreach (Vector3 pos in rayPositions)
    //    {
    //        if (Physics.Raycast(pos, movement, out RaycastHit hit, scope))
    //        {
    //            if (hit.collider.CompareTag("Wall"))
    //                return true;
    //        }
    //    }
    //    return false;
    //}
}
