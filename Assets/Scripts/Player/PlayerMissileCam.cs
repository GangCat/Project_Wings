using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMissileCam : MonoBehaviour
{

    private Transform playerTr;
    [SerializeField]
    private LayerMask layerMask;
    CameraMovement cam;
    public void Init(Transform _playerTr)
    {
        playerTr = _playerTr;
        cam = Camera.main.GetComponent<CameraMovement>();
    }
    private void Update()
    {
        Collider[] missiles = Physics.OverlapSphere(playerTr.position, 10000f, layerMask);
        if (missiles != null)
        {
            foreach (Collider missile in missiles)
            {
                Vector3 missilePosition = missile.transform.position;
                float distance = Vector3.Distance(playerTr.position, missilePosition);

                if (distance < 800)
                {
                    float targetOffset = Mathf.Lerp(10, 30, 5f * Time.deltaTime);
                    cam.offset = Mathf.Lerp(cam.offset, targetOffset, Time.fixedDeltaTime);
                }
                else
                {
                }
            }

        }
    }
}
