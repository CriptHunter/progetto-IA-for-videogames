using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundBehaviour : MonoBehaviour
{
    public bool looking = false;
    public bool playerFound = false;
    public float resampleTime = 5f;

    public void StartLooking()
    {
        looking = true;
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (!looking)
            return;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        looking = false;
    }

}
