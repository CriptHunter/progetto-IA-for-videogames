using UnityEngine;
using System.Collections;
using TMPro;

public class BossFSM : MonoBehaviour {
	public float reactionTime = 3f; //FSM update time
    public Transform player;
	private FSM fsm; 
    public float listeningRange = 6; //range in which the player is located even without being seen
    public float attackRange = 3;
    public TextMeshProUGUI fsmCurrentTxt; //UI text displaying current FSM state

    private LookAroundBehaviour lookB;
    private PatrolBehaviour patrolB;
    private ChaseBehaviour chaseB;
    private AttackBehaviour atkB;
    private ConeVision coneVision;
    private Velocity velocity;
    private HealthBar health;

	void Start ()
    {
        //VARIABLES
        lookB = GetComponent<LookAroundBehaviour>();
        patrolB = GetComponent<PatrolBehaviour>();
        chaseB = GetComponent<ChaseBehaviour>();
        atkB = GetComponent<AttackBehaviour>();
        coneVision = GetComponent<ConeVision>();
        fsmCurrentTxt = fsmCurrentTxt.GetComponent<TextMeshProUGUI>();
        velocity = GetComponent<Velocity>();
        health = GetComponent<HealthBar>();

        //STATES
        FSMState Start = new FSMState();
        FSMState lookAround = new FSMState();
        FSMState patrol = new FSMState();
        FSMState lastPosition = new FSMState();
        FSMState chase = new FSMState();
        FSMState attack = new FSMState();


        //ACTIONS
        Start.enterActions.Add(StartAction);

        lookAround.enterActions.Add(LookAround);
        lookAround.exitActions.Add(lookB.StopLooking);

        patrol.enterActions.Add(Patrol);
        patrol.exitActions.Add(patrolB.StopPatrol);

        chase.enterActions.Add(Chase);
        chase.exitActions.Add(chaseB.StopAtLastKnowPosition);

        lastPosition.enterActions.Add(LastPosition);

        attack.enterActions.Add(chaseB.StopNow);
        attack.enterActions.Add(Attack);
        attack.exitActions.Add(atkB.StopAttacking);

        //TRANSITIONS
        FSMTransition t0 = new FSMTransition(AlwaysTrue);
        FSMTransition t1 = new FSMTransition(PlayerHidden);
        FSMTransition t2 = new FSMTransition(PlayerDetected);
        FSMTransition t3 = new FSMTransition(PlayerNotDetected);
        FSMTransition t4 = new FSMTransition(PlayerInAttackRange);
        FSMTransition t5 = new FSMTransition(PlayerNotInAttackRange);
        FSMTransition t6 = new FSMTransition(PatrolingFinished);
        FSMTransition t7 = new FSMTransition(NotMoving);
        FSMTransition t8 = new FSMTransition(PlayerNotHidden);

        //LINK STATE - TRANSITION
        Start.AddTransition(t0, patrol);

        lookAround.AddTransition(t1, patrol);
        lookAround.AddTransition(t8, chase);
        lookAround.AddTransition(t2, chase);

        patrol.AddTransition(t2, chase);
        patrol.AddTransition(t6, lookAround);

        chase.AddTransition(t3, lastPosition);
        chase.AddTransition(t4, attack);

        lastPosition.AddTransition(t7, lookAround);
        lastPosition.AddTransition(t2, chase);

        attack.AddTransition(t5, chase);

        //INITIAL STATE
        fsm = new FSM(Start);
		StartCoroutine(Run());
	}

	public IEnumerator Run() {
		while(true) {
            yield return new WaitForSeconds(reactionTime);
            fsm.Update();
		}
	}

    public bool AlwaysTrue()
    {
        return true;
    }

    public bool NotMoving() //the agent is not moving
    {
        return velocity.GetVelocity() == 0;
    }

    public bool PlayerHidden() //if the agent finished searching and didn't find the player
    {
        return !lookB.isPlayerFound() && !lookB.isLooking();
    }

    public bool PlayerNotHidden() //if the agent found the player while searching
    {
        return lookB.isLooking() && lookB.isPlayerFound();
    }

    public bool PatrolingFinished() //if the agent reached the target patrol point
    {
        return patrolB.isPatrolingFinished();
    }

    //if the player is detected:
    // - because he is seen
    // - because he is in the listening range
    // - because the agent is hit
    public bool PlayerDetected()
    {
        return coneVision.Listen(listeningRange) || coneVision.Look() || health.CheckDamage();
    }

    public bool PlayerNotDetected()
    {
        return !PlayerDetected();
    }

    public bool PlayerInAttackRange()
    {
        Vector3 ray = player.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, ray, out hit, attackRange))
            if (hit.transform == player)
                return true;
        return false;
    }

    public bool PlayerNotInAttackRange()
    {
        return !PlayerInAttackRange();
    }

    //ACTIONS
    public void StartAction()
    {
        fsmCurrentTxt.text = "Start";
    }

    public void LookAround()
    {
        fsmCurrentTxt.text = "Look around";
        lookB.StartLooking();
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

    public void Chase()
    {
        fsmCurrentTxt.text = "Chase";
        chaseB.StartChasing();
    }

    public void Attack()
    {
        fsmCurrentTxt.text = "Attack";
        atkB.StartAttacking();
    }
}