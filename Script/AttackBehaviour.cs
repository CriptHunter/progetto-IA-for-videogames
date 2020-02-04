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
        if(attacking) //if in attack states, agent looks at the player
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
        StartBT(); //restart again the tree
    }

    //CONDITIONS
    private bool PlayerAttacking()
    {
        return player.GetComponentInChildren<MeleeAttack>().IsChargingAttack();
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

    private bool GetCloser() //agents reaches player position
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
}
