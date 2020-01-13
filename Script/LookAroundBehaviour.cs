using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundBehaviour : MonoBehaviour
{
    public bool looking = false;
    public bool playerFound = false;
    public float resampleTime = 5f;

    void Start()
    {
        
    }
    
    private IEnumerator LookAround()
    {

        yield return new WaitForSeconds(resampleTime);
    }

}
