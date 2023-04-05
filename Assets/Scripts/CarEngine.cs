using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform[] rearWheels;
    [SerializeField] Transform[] frontWheels;
    private Transform[] wheels;
    [SerializeField] bool rearWheelDrive;
    [SerializeField] float horsepower;
    [SerializeField] float maxSteeringAngle;
    private Vector3 currentSteeringAngle;
    [SerializeField] float brakeForce;

    private void Start()
    {
        if (rearWheelDrive)
        {
            wheels = rearWheels;
        }
        else { wheels = frontWheels; }

        currentSteeringAngle = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        currentSteeringAngle = Vector3.Lerp(currentSteeringAngle, (Vector3.up * maxSteeringAngle * Input.GetAxis("Horizontal")), 1);
    }

    private void FixedUpdate()
    {
        foreach (Transform wheel in wheels)
        {
            rb.AddForceAtPosition(wheel.transform.forward * horsepower * Input.GetAxis("Vertical"), wheel.transform.position);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyBrakes();
        }

        // steering
        foreach (Transform wheel in frontWheels)
        {
            wheel.localEulerAngles = currentSteeringAngle;
        }
    }

    private void ApplyBrakes()
    {
        foreach (Transform wheel in wheels)
        {
            float forwardVelocity = Vector3.Dot(rb.GetPointVelocity(wheel.transform.position), wheel.transform.forward);
            //rb.AddForceAtPosition(brakeForce * new Vector3(0, forwardVelocity,0).normalized, wheel.transform.position);
            if (forwardVelocity > 0.1)
            {
                rb.AddForceAtPosition(brakeForce * -wheel.transform.forward, wheel.transform.position);
            }
            else if(forwardVelocity < 0.1)
            {
                rb.AddForceAtPosition(brakeForce * wheel.transform.forward, wheel.transform.position);
            }
        }
    }
}
