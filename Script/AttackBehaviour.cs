using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    private Coroutine c;
    private CharacterController playerCtrl;
    private bool pushing = false;
    private bool attacking = false;
    [SerializeField] private Transform player;
    [SerializeField] private float atkSpeed = 2f;


    void Start()
    {
        playerCtrl = player.gameObject.GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (attacking) //se è nello stato di attacco si gira sempre verso il giocatore
            transform.LookAt(player);
        if(pushing) //spinge il giocatore con un attacco
            playerCtrl.Move((transform.forward.normalized*10 + Vector3.up*15) * Time.fixedDeltaTime);
    }

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
        attacking = true;
        while (true)
        {
            StartCoroutine(Push());
            yield return new WaitForSeconds(atkSpeed);
        }
    }

    private IEnumerator Push()
    {
        pushing = true;
        yield return new WaitForSeconds(0.3f);
        pushing = false;
    }
}
