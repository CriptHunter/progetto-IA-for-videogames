using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeVision : MonoBehaviour
{

    [SerializeField] private float fov = 120f;
    [SerializeField] private float distance = 20f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerMask; //mask with only the layer player

    public bool Look()
    {
        if (Vector3.Distance(transform.position, player.position) < distance) //if the player is in sight range
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized; //ray from player to agent
            if (Vector3.Angle(transform.forward, dirToPlayer) < fov/2) //if the player is inside the cone of vision
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dirToPlayer, out hit)) //if the player is visible (not behind walls or obstacles)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        return true;
                }
            }
        }

        return false;
    }

    public bool Listen(float range) //check if player is near, not considering walls or obstacles
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
