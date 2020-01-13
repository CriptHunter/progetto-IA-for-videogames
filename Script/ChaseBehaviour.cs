using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class ChaseBehaviour : MonoBehaviour {

	public Transform player;
	public float resampleTime = 5f;
    private Coroutine c;

    public void StartChasing()
    {
        c = StartCoroutine(GoChasing());
    }

    public void StopChasing()
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
            GetComponent<NavMeshAgent>().destination = player.position;
            yield return new WaitForSeconds(resampleTime);
        }
    }
}
