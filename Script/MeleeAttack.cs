using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private bool chargingAttack = false;
    [SerializeField] private float chargingTime = 2f;
    private float timer = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            timer = 0;
            chargingAttack = true;
        }

        else if (Input.GetKeyUp(KeyCode.R))
        {
            chargingAttack = false;
            if (timer >= 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
                {
                    HealthBar h = hit.collider.GetComponent<HealthBar>();
                    if (h != null)
                        h.TakeDamage(40);
                }
            }
        }

        timer = timer + Time.deltaTime;

    }

    public bool IsChargingAttack()
    {
        return chargingAttack;
    }


}
