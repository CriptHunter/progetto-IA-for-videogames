using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    private int health = 100;
    private bool damaged = false; //se il boss è stato colpito rispetto all'ultima volta che si è controllato

    public bool CheckDamage()
    {
        bool tmp = damaged;
        damaged = false;
        return tmp;
    }

    public void TakeDamage(int dmg)
    {
        damaged = true;
        health = health - dmg;
        if (health <= 0)
        {
            Destroy(this.gameObject);
            barImage.fillAmount = 0;
        }
        barImage.fillAmount = (float)health/100;
        print("vita:" + health);
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
