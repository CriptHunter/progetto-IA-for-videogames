using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundBehaviour : MonoBehaviour
{
    private bool looking = false;
    private bool playerFound = false;
    [SerializeField] private float rotationSpeed = 5;
    private Coroutine c;
    private ConeVision coneVision;

    private void Start()
    {
        coneVision = GetComponent<ConeVision>();
    }

    public bool isLooking()
    {
        return looking;
    }

    public bool isPlayerFound()
    {
        return playerFound;
    }

    public void StartLooking()
    {
        looking = true;
        c = StartCoroutine(Look(90));
    }

    public void StopLooking()
    {
        looking = false;
        StopCoroutine(c);
    }

    private IEnumerator Look(float rotationAmount)
    {
        //looks to the right
        Quaternion finalRotation = Quaternion.Euler(0, rotationAmount, 0) * this.transform.rotation;
        while (transform.rotation != finalRotation)
        {
            playerFound = coneVision.Look();
            this.transform.rotation = Rotate(finalRotation, rotationSpeed);
            yield return 0;
        }

        //back looking forward
        finalRotation = Quaternion.Euler(0, -rotationAmount, 0) * this.transform.rotation;
        while (transform.rotation != finalRotation)
        {
            playerFound = coneVision.Look();
            this.transform.rotation = Rotate(finalRotation, rotationSpeed);
            yield return 0;
        }

        //look to the left
        finalRotation = Quaternion.Euler(0, -rotationAmount, 0) * this.transform.rotation;
        while (transform.rotation != finalRotation)
        {
            playerFound = coneVision.Look();
            this.transform.rotation = Rotate(finalRotation, rotationSpeed);
            yield return 0;
        }

        looking = false;
    }

    private Quaternion Rotate(Quaternion fRotation, float speed)
    {
        return Quaternion.Lerp(this.transform.rotation, fRotation, Time.deltaTime * speed);
    }

}
