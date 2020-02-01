using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightAround : MonoBehaviour
{
    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask lightMask;
    private float baseSpeed = 0;
    private float actualSpeed = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;
        StartCoroutine(CountLight()); 
    }

    private IEnumerator CountLight()
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, lightMask);
            int lightNumber = 0;
            foreach (Collider c in hitColliders)
            { 
                LightSource2 l = c.GetComponent<LightSource2>();
                if (l != null && l.isLit())
                    lightNumber++;
            }
            agent.speed = baseSpeed - 0.75f * lightNumber;
            yield return new WaitForSeconds(3);
        }
    }
}
