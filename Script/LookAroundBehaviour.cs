using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundBehaviour : MonoBehaviour
{
    public bool looking = false;
    public bool playerFound = false;
    [SerializeField] private float rotationSpeed = 5;
    private Coroutine c;
    private ConeVision coneVision;

    private void Start()
    {
        coneVision = GetComponent<ConeVision>();
    }

    public void isLooking()
    {

    }

    public void isPlayerFound()
    {

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
        //guarda a destra
        Quaternion finalRotation = Quaternion.Euler(0, rotationAmount, 0) * this.transform.rotation;
        while (transform.rotation != finalRotation)
        {
            playerFound = coneVision.Look();
            this.transform.rotation = Rotate(finalRotation, rotationSpeed);
            yield return 0;
        }

        //torna al centro
        finalRotation = Quaternion.Euler(0, -rotationAmount, 0) * this.transform.rotation;
        while (transform.rotation != finalRotation)
        {
            playerFound = coneVision.Look();
            this.transform.rotation = Rotate(finalRotation, rotationSpeed);
            yield return 0;
        }

        //guarda a sinistra
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
