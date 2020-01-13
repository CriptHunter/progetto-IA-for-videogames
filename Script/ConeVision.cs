using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeVision : MonoBehaviour
{

    public float fov = 120f;
    public float distance = 20f;
    public Transform player;

    public bool Look()
    {
        if (Vector3.Distance(transform.position, player.position) < distance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < fov/2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dirToPlayer, out hit))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        return true;
                }
            }
        }

        return false;
    }

    private void Update()
    {
        Look();
    }
}
