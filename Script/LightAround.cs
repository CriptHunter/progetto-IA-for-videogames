using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightAround : MonoBehaviour
{
    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask lightMask;
    [SerializeField] private LayerMask wallMask;
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
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, lightMask); //check all lights in a sphere of radius r
            int lightNumber = 0;
            foreach (Collider c in hitColliders)
            {
                RaycastHit hit;
                //check if the light is not behind a wall
                if (Physics.Raycast(transform.position, (c.transform.position - transform.position), out hit, Mathf.Infinity, wallMask))
                {
                    LightSource2 l = hit.collider.GetComponent<LightSource2>();
                    if (l != null && l.isLit()) //if the light is ON, increases light count
                        lightNumber++;
                }
            }
            agent.speed = baseSpeed - 0.75f * lightNumber; //decrease agent speed based on lights number
            yield return new WaitForSeconds(3);
        }
    }
}
