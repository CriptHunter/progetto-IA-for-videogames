using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]

public class PatrolBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private int index;
    [SerializeField] private List<GameObject> patrolingPoint;
    private int[] weights;
    private bool patrolingFinished;
    [SerializeField] private float resampleTime = 5f;
    private Coroutine c;
    [SerializeField] private bool wavePatrol;

    void Start()
    {
        index = -1;
        patrolingFinished = false;
        agent = GetComponent<NavMeshAgent>();
        weights = new int[patrolingPoint.Count];
        InitWeights();
    }

    public bool isPatrolingFinished()
    {
        return patrolingFinished;
    }

    public void StartPatrol()
    {
        if (patrolingPoint.Count == 0)
            return;

        int sum = 0;
        foreach (int e in weights) //sum of all weights
            sum = sum + e;

        if (sum == 0) //if the sum of the weights is zero, it means that all the patrol points have been chosen 1 time
            InitWeights(); //sets all probability to 1

        index = GetRandomWeightedIndex(weights); //choose a patrol point
        weights[index] = 0; //sets to 0 the probability of going there another time
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
            if (Vector3.Distance(transform.position, patrolingPoint[index].transform.position) <= 3) //stops near patroling point
            {
                agent.destination = transform.position;
                patrolingFinished = true;
                print("finished patrol");
            }
            else if (Vector3.Distance(transform.position, patrolingPoint[index].transform.position) <= 10) //if close to patroling points the agent starts to walk straight
                agent.destination = patrolingPoint[index].transform.position;
            else
            {
                float sinOffset;
                if (wavePatrol)
                    sinOffset = Mathf.Sin(Time.time) * Vector3.Distance(transform.position, patrolingPoint[index].transform.position);
                else
                    sinOffset = 0;

                agent.destination = patrolingPoint[index].transform.position + transform.right * sinOffset;
            }
            yield return new WaitForSeconds(resampleTime);
        }
    }

    public int GetRandomWeightedIndex(int[] weights)
    {
        int weightSum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            weightSum += weights[i];
        }
        int index = 0;
        int lastIndex = weights.Length - 1;
        while (index < lastIndex)
        {
            if (Random.Range(0, weightSum) < weights[index])
            {
                return index;
            }
            weightSum -= weights[index++];
        }
        return index;
    }

    private void InitWeights()
    {
       for(int k = 0; k < weights.Length; k++)
       {
            weights[k] = 1;
       }
    }
}
