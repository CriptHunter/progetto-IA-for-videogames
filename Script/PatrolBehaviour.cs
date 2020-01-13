using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]

public class PatrolBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private int index;
    public List<GameObject> patrolingPoint;
    public bool patrolingFinished;
    public float resampleTime = 5f;
    Coroutine c;

    void Start()
    {
        index = -1;
        patrolingFinished = false;
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartPatrol()
    {
        if (patrolingPoint.Count == 0)
            return;

        if (index >= patrolingPoint.Count -1)
            index = -1;

        index++;
        patrolingFinished = false;
        agent.destination = patrolingPoint[index].transform.position;

        c = StartCoroutine(Patrol());
        
    }

    public void StopPatrol()
    {
        StopCoroutine(c);
    }

    private IEnumerator Patrol()
    {
        while(true)
        {
            if (Vector3.Distance(transform.position, patrolingPoint[index].transform.position) <= agent.stoppingDistance)
            {
                patrolingFinished = true;
                print("finished patrol");
            }
            yield return new WaitForSeconds(resampleTime);
        }
    }
}
