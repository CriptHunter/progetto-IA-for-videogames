using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class ChaseBehaviour : MonoBehaviour
{
	[SerializeField] private Transform player;
    [SerializeField] private float frequency = 3;
    [SerializeField] private float amplitude = 3;
    [SerializeField] private float speed = 5;
    [SerializeField] private float resampleTime = 2;

    private Coroutine c;
    private NavMeshAgent agent;
    private float baseSpeed;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;
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
        agent.speed = baseSpeed;
        print("stopping now");
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
