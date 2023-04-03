using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] Transform[] wheels;
    [SerializeField] float suspensionRestDist;
    [SerializeField] float springPower;
    [SerializeField] float springDamper;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.4f, 0);
        foreach (var wheel in wheels)
        {
            wheel.gameObject.AddComponent<SuspensionForce>().Initialize(suspensionRestDist, springPower, springDamper);
        }
    }
}
