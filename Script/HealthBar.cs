﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    private int health = 100;
    private bool damaged = false;

    public bool CheckDamage() //true if the agent was hit after the last check
    {
        bool tmp = damaged;
        damaged = false;
        return tmp;
    }

    public void TakeDamage(int dmg) //decrease agent health
    {
        if (GetComponent<AttackBehaviour>().blocking)
        {
            print("blocking...");
            return;
        }

        damaged = true;
        health = health - dmg;
        if (health <= 0)
        {
            Destroy(this.gameObject);
            barImage.fillAmount = 0;
        }
        barImage.fillAmount = (float)health/100;
    }

    public int GetHealth()
    {
        return health;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LightBullet>() != null)
            TakeDamage(20);
    }
}
