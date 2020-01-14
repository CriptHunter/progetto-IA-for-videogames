using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundBehaviour : MonoBehaviour
{
    public bool looking = false;
    public bool playerFound = false;
    public float resampleTime = 5f;
    private float rightRotation = 60;
    private float leftRotation = -60;


    public void StartLooking()
    {

    }

    public void StopLooking()
    {
        looking = false;
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

    public void LookRight()
    {

    }

}
