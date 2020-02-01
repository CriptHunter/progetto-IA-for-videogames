using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CRBT;
using UnityEngine.AI;

public class AttackBehaviour : MonoBehaviour
{
    private BehaviorTree bt;
    private BTSelector root;
    private Coroutine c;
    private CharacterController playerCtrl;
    private bool attacking = false;
    [SerializeField] private Transform player;
    [SerializeField] private float treeUpdateTime = 2f;
    [SerializeField] private float playerMeleeDistance = 2f;

    void Start()
    {
        //INIT
        playerCtrl = player.gameObject.GetComponent<CharacterController>();

        //ACTIONS
        BTAction block = new BTAction(Block);
        BTAction meleeAttack = new BTAction(MeleeAttack);
        BTAction rangedAttack = new BTAction(RangedAttack);
        BTAction getCloser = new BTAction(GetCloser);
        
        //CONDITIONS
        BTCondition playerAttacking = new BTCondition(PlayerAttacking);
        BTCondition playerMelee = new BTCondition(PlayerMelee);
        BTCondition playerFar = new BTCondition(PlayerFar);

        //TREE
        BTSequence seqBlock = new BTSequence(new IBTTask[] {playerAttacking, block});
        BTSequence seqMelee = new BTSequence(new IBTTask[] {playerMelee, meleeAttack});
        BTSequence seqGetCloser = new BTSequence(new IBTTask[] { playerFar, getCloser});
        BTDecorator untilFailGetCloser = new BTDecoratorUntilFail(seqGetCloser);
        BTRandomSelector rselRanged = new BTRandomSelector(new IBTTask[] { untilFailGetCloser, rangedAttack });
        BTSelector selAttack = new BTSelector(new IBTTask[] {seqMelee, rselRanged});
        root = new BTSelector(new IBTTask[] {seqBlock, selAttack});
    }

    void Update()
    {
        if(attacking)
            transform.LookAt(player.position);
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
        return (rand > 0.5f);
    }

    private bool PlayerMelee()
    {
        if (Vector3.Distance(player.position, transform.position) <= playerMeleeDistance)
            return true;
        return false;
    }

    private bool PlayerFar()
    {
        return !PlayerMelee();
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

    private bool GetCloser()
    {
        GetComponent<NavMeshAgent>().destination = player.position;
        return true;
    }

    //OTHER
    public void StartAttacking()
    {
        attacking = true;
        StartBT();
    }

    public void StopAttacking()
    {
        attacking = false;
        StopAllCoroutines();
    }

    /*
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
    }*/
}
