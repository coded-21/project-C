using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspensionForce : MonoBehaviour
{
    [SerializeField] Rigidbody carRb;
    [SerializeField] LayerMask layerMask;
    private bool initialized;

    private float suspensionRestDist;
    private float springPower;
    private float springDamper;

    public void Initialize(float _suspensionRestDist, float _springPower, float _springDamper)
    {

        suspensionRestDist = _suspensionRestDist;
        springPower = _springPower;
        springDamper = _springDamper;

        initialized = true;
    }

    private void Awake()
    {
        carRb = GetComponentInParent<Rigidbody>();
        layerMask = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate()
    {
        if (!initialized) return;

        Ray ray = new Ray(transform.position, -transform.up);

        // check if ray hit ground
        if (Physics.Raycast(ray, out RaycastHit hit, suspensionRestDist, layerMask))
        {
            // Spring Force
            float offset = (suspensionRestDist - hit.distance)/suspensionRestDist;
            float springForce = springPower * offset;

            // Damping Force
            Vector3 tireWorldVel = carRb.GetPointVelocity(transform.position);
            float vel = Vector3.Dot(tireWorldVel, transform.up);
            float dampingForce = vel * springDamper;

            // Suspension Force
            float suspensionForce = springForce - dampingForce;
            carRb.AddForceAtPosition(transform.up * suspensionForce, transform.position);
        }
        Debug.DrawRay(transform.position, -transform.up, Color.green, Time.deltaTime);
    }
}
