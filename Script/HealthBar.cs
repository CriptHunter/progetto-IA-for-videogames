using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    private int health = 100;

    public void TakeDamage(int dmg)
    {
        
        if (dmg < 0)
            dmg = -dmg;
        health = health - dmg;
        if (health <= 0)
        {
            Destroy(this.gameObject);
            barImage.fillAmount = 0;
        }
        barImage.fillAmount = (float)health/100;
        print("vita:" + health);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LightBullet>() != null)
            TakeDamage(20);
    }
}
