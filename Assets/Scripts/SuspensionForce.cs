using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspensionForce : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask layerMask;

    [SerializeField] float suspensionRestDist;
    [SerializeField] float springPower;
    [SerializeField] float springDamper;

    private void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        // check if ray hit ground
        if (Physics.Raycast(ray, out RaycastHit hit, suspensionRestDist, layerMask))
        {
            // Spring Force
            float offset = suspensionRestDist - hit.distance;
            float springForce = springPower * offset;

            // Damping Force
            Vector3 tireWorldVel = rb.GetPointVelocity(transform.position);
            float vel = Vector3.Dot(tireWorldVel, transform.up);
            float dampingForce = vel * springDamper;

            // Suspension Force
            float suspensionForce = springForce - dampingForce;
            rb.AddForceAtPosition(Vector3.up * suspensionForce, transform.position);
        }
        Debug.DrawRay(transform.position, -transform.up*suspensionRestDist, Color.green, Time.deltaTime);
    }
}
