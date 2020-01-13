using UnityEngine;
using System.Collections;

public class BossFSM : MonoBehaviour {
	public float reactionTime = 3f;
    public Transform player;
	private FSM fsm;
    public LayerMask playerMask;
    public float listeningRange = 4;


	void Start ()
    {
        //STATES
        FSMState lookAround = new FSMState();
        FSMState patrol = new FSMState();
        FSMState chase = new FSMState();
        FSMState attack = new FSMState();

        //ACTIONS
        lookAround.enterActions.Add(LookAround);
        patrol.enterActions.Add(Patrol);
        chase.enterActions.Add(Chase);
        chase.exitActions.Add(StopChase);
        attack.enterActions.Add(Attack);
        attack.stayActions.Add(Attack);

        //TRANSITIONS
        FSMTransition t1 = new FSMTransition(PlayerAround);
        FSMTransition t2 = new FSMTransition(PlayerInSight);
        FSMTransition t3 = new FSMTransition(PlayerNotInSight);

        //LINK STATE - TRANSITION
        lookAround.AddTransition(t1, patrol);
        patrol.AddTransition(t2, chase);
        chase.AddTransition(t3, lookAround);

        //INITIAL STATE
        fsm = new FSM(lookAround);
		StartCoroutine(Run());
	}

	// Periodic update, run forever
	public IEnumerator Run() {
		while(true) {
			fsm.Update();
			yield return new WaitForSeconds(reactionTime);
		}
	}

    //CONDITION
    public bool PlayerAround()
    {
        return true;
    }

    public bool NothingAround()
    {
        return !PlayerAround();
    }

    public bool PlayerInSight()
    {
        if (PlayerInListeningRange())
        {
            return true;
        }

        //se il player non è abbastanza vicino da essere sentito prova a guardare
        Vector3 ray = player.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, ray, out hit))
        {
            if (hit.transform == player)
            {
                return true;
            }
        }
        return false;
    }

    public bool PlayerNotInSight()
    {
        return !PlayerInSight();
    }

    //true se il giocatore è abbastanza vicino da essere sentito
    //l'agente non deve per forza vederlo
    public bool PlayerInListeningRange()
    {
        Vector3 ray = player.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, ray, out hit, 4, playerMask))
        {
            if (hit.transform == player)
            {
                return true;
            }
        }
        return false;
    }

    //ACTIONS
    public void LookAround()
    {
        print("looking around...");
    }

    public void Patrol()
    {
        print("patrol...");
    }

    public void Chase()
    {
        print("chase...");
        GetComponent<ChaseBehaviour>().StartChasing();
    }

    public void StopChase()
    {
        print("stop chasing...");
        GetComponent<ChaseBehaviour>().StopChasing();
    }

    public void Attack()
    {
        print("attack...");
    }


/*
	// CONDITIONS
	public bool EnemiesAround() {
		foreach (GameObject go in GameObject.FindGameObjectsWithTag(targetTag)) {
			if ((go.transform.position - transform.position).magnitude <= range) return true;
		}
		return false;
	}

	public bool NoEnemiesAround() {
		return !EnemiesAround();
	}

	// ACTIONS

	public void StartAlarm () {
		initialColor = ambientLight.color;
		ringStart = Time.realtimeSinceStartup;
	}

	public void ShutAlarm() {
		ambientLight.color = initialColor;
	}

	public void RingAlarm() {
		if ((int)Mathf.Floor ((Time.realtimeSinceStartup - ringStart) / switchTime) % 2 == 0) {
			ambientLight.color = color1;
		} else {
			ambientLight.color = color2;
		}
	}

*/

}