using UnityEngine;
using System.Collections;
using TMPro;

public class BossFSM : MonoBehaviour {
	public float reactionTime = 3f; //ogni quanti secondi la FSM si aggiorna
    public Transform player; //player da inseguire
	private FSM fsm; 
    public float listeningRange = 6; //range entro il quale il player viene sentito anche senza essere visto
    public float attackRange = 3; //range di attacco
    public TextMeshProUGUI fsmCurrentTxt; //testo sulla UI con lo stato corrente della FSM

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

        //TRANSITIONS
        FSMTransition t0 = new FSMTransition(AlwaysTrue);
        FSMTransition t1 = new FSMTransition(PlayerHidden);
        FSMTransition t2 = new FSMTransition(PlayerInSight);
        FSMTransition t3 = new FSMTransition(PlayerNotInSight);
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

    //CONDITION
    public bool AlwaysTrue()
    {
        return true;
    }

    //il boss è fermo
    public bool NotMoving()
    {
        return velocity.GetVelocity() == 0;
    }

    //se mentre cerca vede il giocatore
    public bool PlayerNotHidden()
    {
        return lookB.looking && lookB.playerFound;
    }

    //se ha finito di cercare e non ha visto il giocatore
    public bool PlayerHidden()
    {
        return !lookB.playerFound && !lookB.looking;
    }

    //se è arrivato con successo ad un punto di patrol
    public bool PatrolingFinished()
    {
        return patrolB.patrolingFinished;
    }

    //se il giocatore viene visto o sentito
    public bool PlayerInSight()
    {
        //se il giocatore è abbastanza vicino da essere sentito
        if (coneVision.Listen(listeningRange))
            return true;

        return coneVision.Look();
    }

    //se il giocatore non viene visto o sentito
    public bool PlayerNotInSight()
    {
        return !PlayerInSight();
    }

    //se il giocatore è abbastanza vicino da essere attaccato
    public bool PlayerInAttackRange()
    {
        Vector3 ray = player.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, ray, out hit, attackRange))
            if (hit.transform == player)
                return true;
        return false;
    }

    //se il giocatore è troppo lontano per essere attaccato
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
    }

    //UTILS


}