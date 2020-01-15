using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundBehaviour : MonoBehaviour
{
    public bool looking = false;
    public bool playerFound = false;
    public float rotationSpeed = 5;
    private Coroutine c;


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
            this.transform.rotation = Rotate(finalRotation, rotationSpeed);
            yield return 0;
        }

        //torna al centro
        finalRotation = Quaternion.Euler(0, -rotationAmount, 0) * this.transform.rotation;
        while (transform.rotation != finalRotation)
        {
            this.transform.rotation = Rotate(finalRotation, rotationSpeed);
            yield return 0;
        }

        //guarda a sinistra
        finalRotation = Quaternion.Euler(0, -rotationAmount, 0) * this.transform.rotation;
        while (transform.rotation != finalRotation)
        {
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
