using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class ChaseBehaviour : MonoBehaviour
{
	[SerializeField] private Transform player;
    [SerializeField] private float resampleTime = 2;

    private Coroutine c;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartChasing()
    {
        c = StartCoroutine(GoChasing());
    }

    public void StopAtLastKnowPosition()
    {
        StopCoroutine(c);
    }

    public void StopNow()
    {
        StopCoroutine(c);
        agent.destination = transform.position;
    }

    private IEnumerator GoChasing()
    {
        while (true)
        {
            agent.destination = player.position;
            yield return new WaitForFixedUpdate();
        }
    }
}
