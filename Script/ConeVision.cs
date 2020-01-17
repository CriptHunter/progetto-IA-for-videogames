using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeVision : MonoBehaviour
{

    [SerializeField] private float fov = 120f;
    [SerializeField] private float distance = 20f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerMask; //maschera con solo il layer player

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

    public bool Listen(float range)
    {
        Vector3 ray = player.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, ray, out hit, range, playerMask))
        {
            if (hit.transform == player)
            {
                print("player listened");
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        Look();
    }
}
