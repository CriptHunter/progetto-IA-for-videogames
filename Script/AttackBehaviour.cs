using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    private Coroutine c;
    [SerializeField] private Transform player;
    [SerializeField] private float atkSpeed = 2f;


    public void StartAttacking()
    {
        c = StartCoroutine(Attack(atkSpeed));
    }

    public void StopAttacking()
    {
        StopCoroutine(c);
    }
    
    private IEnumerator Attack(float atkSpeed)
    {
        while (true)
        {
            print("attacking!");
            player.gameObject.GetComponent<CharacterController>().Move(transform.forward * 2);
            yield return new WaitForSeconds(atkSpeed);
        }
    }
}
