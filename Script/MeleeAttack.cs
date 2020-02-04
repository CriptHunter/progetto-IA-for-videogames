using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private bool chargingAttack = false;

    void Update()
    {
        print(chargingAttack);
        if (Input.GetKey(KeyCode.R))
            chargingAttack = true;
        else
            chargingAttack = false;
    }

    public bool IsChargingAttack()
    {
        return chargingAttack;
    }


}
