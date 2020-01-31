using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CRBT;

public class AttackBehaviour : MonoBehaviour
{
    private BehaviorTree bt;
    private BTSelector root;
    private Coroutine c;
    private CharacterController playerCtrl;
    private bool pushing = false;
    private bool attacking = false;
    [SerializeField] private Transform player;
    [SerializeField] private float atkSpeed = 2f;
    [SerializeField] private float treeUpdateTime = 2f;

    void Start()
    {
        //INIT
        playerCtrl = player.gameObject.GetComponent<CharacterController>();

        //ACTIONS
        BTAction block = new BTAction(Block);
        BTAction meleeAttack = new BTAction(MeleeAttack);
        BTAction rangedAttack1 = new BTAction(RangedAttack);
        BTAction rangedAttack2 = new BTAction(RangedAttack);
        
        //CONDITIONS
        BTCondition playerAttacking = new BTCondition(PlayerAttacking);
        BTCondition playerMelee = new BTCondition(PlayerMelee);

        //COMPOSITE
        BTSequence seqBlock = new BTSequence(new IBTTask[] {playerAttacking, block});
        BTSequence seqMelee = new BTSequence(new IBTTask[] {playerMelee, meleeAttack});
        BTSequence seqRanged = new BTSequence(new IBTTask[] {rangedAttack1, rangedAttack2});
        BTSelector selAttack = new BTSelector(new IBTTask[] {seqMelee, seqRanged});
        root = new BTSelector(new IBTTask[] {seqBlock, selAttack});

        //START
        StartBT();
    }

    private void StartBT()
    {
        bt = new BehaviorTree(root);
        StartCoroutine(AttackTree());
    }

    

    private IEnumerator AttackTree()
    {
        while (bt.Step())
        {
            yield return new WaitForSeconds(treeUpdateTime);
        }
        StartBT();
    }

    //CONDITIONS
    private bool PlayerAttacking()
    {
        float rand = Random.value;
        print(rand);
        return (rand > 0.5f);
    }

    private bool PlayerMelee()
    {
        if (Vector3.Distance(player.position, transform.position) <= 2)
            return true;
        return false;
    }

    //ACTIONS
    private bool Block()
    {
        print("block");
        return true;
    }

    private bool MeleeAttack()
    {
        print("melee attack");
        return true;
    }

    private bool RangedAttack()
    {
        print("ranged attack");
        return true;
    }



    /*
    private void FixedUpdate()
    {
        if (attacking) //se è nello stato di attacco si gira sempre verso il giocatore
            transform.LookAt(player);
        if(pushing) //spinge il giocatore con un attacco
            playerCtrl.Move((transform.forward.normalized*10 + Vector3.up*15) * Time.fixedDeltaTime);
    }*/

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
