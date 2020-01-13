using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class ChaseBehaviour : MonoBehaviour {

	public Transform player;
	public float resampleTime = 5f;
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

	/*private IEnumerator GoChasing() {
		while (true) {
			Vector3 ray = player.position - transform.position;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, ray, out hit)) {
				if (hit.transform == player) {
					GetComponent<NavMeshAgent> ().destination = player.position;
				}
			}
			yield return new WaitForSeconds (resampleTime);
		}
	}*/

    private IEnumerator GoChasing()
    {
        while (true)
        {
            agent.destination = player.position;
            yield return new WaitForSeconds(resampleTime);
        }
    }
}
