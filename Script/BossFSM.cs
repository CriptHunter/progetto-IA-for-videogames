using UnityEngine;
using System.Collections;
using TMPro;

public class BossFSM : MonoBehaviour {
	public float reactionTime = 3f;
    public Transform player;
	private FSM fsm;
    public LayerMask playerMask;
    public float listeningRange = 6;
    public float attackRange = 3;
    public TextMeshProUGUI fsmCurrentTxt;

    private LookAroundBehaviour lookB;
    private PatrolBehaviour patrolB;
    private ChaseBehaviour chaseB;
    private ConeVision coneVision;
    private Velocity velocity;

	void Start ()
    {
        //VARIABLES
        lookB = GetComponent<LookAroundBehaviour>();
        patrolB = GetComponent<PatrolBehaviour>();
        chaseB = GetComponent<ChaseBehaviour>();
        coneVision = GetComponent<ConeVision>();
        fsmCurrentTxt = fsmCurrentTxt.GetComponent<TextMeshProUGUI>();
        velocity = GetComponent<Velocity>();

        //STATES
        FSMState lookAround = new FSMState();
        FSMState patrol = new FSMState();
        FSMState lastPosition = new FSMState();
        FSMState chase = new FSMState();
        FSMState attack = new FSMState();

        //ACTIONS
        lookAround.enterActions.Add(LookAround);

        patrol.enterActions.Add(Patrol);
        patrol.exitActions.Add(StopPatrol);

        chase.enterActions.Add(Chase);
        chase.exitActions.Add(StopChase);

        lastPosition.enterActions.Add(LastPosition);
        attack.enterActions.Add(Attack);
        attack.stayActions.Add(Attack);
    
        //TRANSITIONS
        FSMTransition t1 = new FSMTransition(NothingAround);
        FSMTransition t2 = new FSMTransition(PlayerInSight);
        FSMTransition t3 = new FSMTransition(PlayerNotInSight);
        FSMTransition t4 = new FSMTransition(PlayerInAttackRange);
        FSMTransition t5 = new FSMTransition(PlayerNotInAttackRange);
        FSMTransition t6 = new FSMTransition(PatrolingFinished);
        FSMTransition t7 = new FSMTransition(NotMoving);


        //LINK STATE - TRANSITION
        lookAround.AddTransition(t1, patrol);
        lookAround.AddTransition(t2, chase);

        patrol.AddTransition(t2, chase);
        patrol.AddTransition(t6, lookAround);

        chase.AddTransition(t3, lastPosition);
        chase.AddTransition(t4, attack);

        lastPosition.AddTransition(t7, lookAround);
        lastPosition.AddTransition(t2, chase);

        attack.AddTransition(t5, chase);

        //INITIAL STATE
        fsm = new FSM(lookAround);
		StartCoroutine(Run());
	}

	public IEnumerator Run() {
		while(true) {
			fsm.Update();
			yield return new WaitForSeconds(reactionTime);
		}
	}

    //CONDITION
    public bool NotMoving()
    {
        print(velocity.velocity);
        return velocity.velocity == 0;
    }

    public bool PlayerAround()
    {
        //se ha trovato il giocatore e non sta cercando
        return lookB.playerFound && !lookB.looking;
    }

    public bool NothingAround()
    {
        return !lookB.playerFound && !lookB.looking;
    }

    public bool PatrolingFinished()
    {
        return patrolB.patrolingFinished;
    }

    public bool PlayerInSight()
    {
        //se il giocatore è abbastanza vicino da essere sentito
        if (PlayerInRange(listeningRange))
        {
            return true;
        }

        return coneVision.Look();
    }

    public bool PlayerNotInSight()
    {
        return !PlayerInSight();
    }

    public bool PlayerInAttackRange()
    {
        return PlayerInRange(attackRange);
    }

    public bool PlayerNotInAttackRange()
    {
        return !PlayerInAttackRange();
    }

    //ACTIONS
    public void LookAround()
    {
        fsmCurrentTxt.text = "Look around";
        lookB.StartLooking();
    }

    public void StopLooking()
    {
        lookB.StopLooking();
    }

    public void LastPosition()
    {
        fsmCurrentTxt.text = "Last position";
    }

    public void Patrol()
    {
        fsmCurrentTxt.text = "Patrol";
        patrolB.StartPatrol();
    }

    public void StopPatrol()
    {
        patrolB.StopPatrol();
    }

    public void Chase()
    {
        fsmCurrentTxt.text = "Chase";
        chaseB.StartChasing();
    }

    public void StopChase()
    {
        chaseB.StopAtLastKnowPosition();
    }

    public void Attack()
    {
        fsmCurrentTxt.text = "Attack";
    }

    //UTILS

    //raycast con distanza solo sul layer del giocatore
    public bool PlayerInRange(float range)
    {
        Vector3 ray = player.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, ray, out hit, range, playerMask))
        {
            if (hit.transform == player)
            {
                return true;
            }
        }
        return false;
    }
}