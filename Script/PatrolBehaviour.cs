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

    /*public void StartPatrol()
    {
        if (patrolingPoint.Count == 0)
            return;

        if (index >= patrolingPoint.Count -1)
            index = -1;

        index++;
        patrolingFinished = false;
        agent.destination = patrolingPoint[index].transform.position;

        c = StartCoroutine(Patrol());       
    }*/

    public void StartPatrol()
    {
        print("start patrol");
        if (patrolingPoint.Count == 0)
            return;

        int sum = 0;
        foreach (int e in weights) //sommo tutti i pesi
            sum = sum + e;

        if (sum == 0) //se la somma dei pesi fa zero significa che tutti i punti di patrol sono stati scelti 1 volta
            InitWeights(); //rimetto le probabilità a 1

        index = GetRandomWeightedIndex(weights); //scelgo random weighted un patrol point
        weights[index] = 0; //metto a zero la probabilità di andarci ancora
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
            print("agent speed: " + agent.speed);

            if (Vector3.Distance(transform.position, patrolingPoint[index].transform.position) <= 3)
            {
                agent.destination = transform.position;
                patrolingFinished = true;
                print("finished patrol");
            }
            else if (Vector3.Distance(transform.position, patrolingPoint[index].transform.position) <= 10)
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
        // Get the total sum of all the weights.
        int weightSum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            weightSum += weights[i];
        }

        // Step through all the possibilities, one by one, checking to see if each one is selected.
        int index = 0;
        int lastIndex = weights.Length - 1;
        while (index < lastIndex)
        {
            // Do a probability check with a likelihood of weights[index] / weightSum.
            if (Random.Range(0, weightSum) < weights[index])
            {
                return index;
            }

            // Remove the last item from the sum of total untested weights and try again.
            weightSum -= weights[index++];
        }

        // No other item was selected, so return very last index.
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
