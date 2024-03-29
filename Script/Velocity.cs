﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    private Vector3 oldPos;
    private Vector3 newPos;
    private Vector3 velocityVector;
    private float velocity;

    void Start()
    {

        oldPos = transform.position;
    }

    void Update()
    {
        newPos = transform.position;
        Vector3 delta = (newPos - oldPos);
        velocityVector = delta / Time.deltaTime; // Δs/Δt 
        velocity = velocityVector.magnitude;
        oldPos = newPos;
    }

    public float GetVelocity()
    {
        return velocity;
    }
}
