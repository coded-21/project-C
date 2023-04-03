using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuspensionForce : MonoBehaviour
{
    [SerializeField] Rigidbody carRb;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform[] wheels;
    [SerializeField] GameObject[] wheelMeshes;

    [SerializeField] private float suspensionRestDist;
    [SerializeField] private float springPower;
    [SerializeField] private float springDamper;
    [SerializeField] private float wheelRadius;

    private void FixedUpdate()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            Ray ray = new Ray(wheels[i].transform.position, -wheels[i].transform.up);

            // check if ray hit ground
            if (Physics.Raycast(ray, out RaycastHit hit, suspensionRestDist, layerMask))
            {
                // Spring Force
                float offset = (suspensionRestDist - hit.distance) / suspensionRestDist;
                float springForce = springPower * offset;

                // Damping Force
                Vector3 tireWorldVel = carRb.GetPointVelocity(wheels[i].transform.position);
                float vel = Vector3.Dot(tireWorldVel, wheels[i].transform.up);
                float dampingForce = vel * springDamper;

                // Suspension Force
                float suspensionForce = springForce - dampingForce;
                carRb.AddForceAtPosition(wheels[i].transform.up * suspensionForce, wheels[i].transform.position);

                wheelMeshes[i].transform.localPosition = new Vector3(
                    wheelMeshes[i].transform.localPosition.x,
                    (wheelRadius - hit.distance) / wheels[i].lossyScale.y,
                    wheelMeshes[i].transform.localPosition.z
                    );
            }
            else
            {
                wheelMeshes[i].transform.localPosition = new Vector3(
                    wheelMeshes[i].transform.localPosition.x,
                    (wheelRadius - suspensionRestDist) / wheels[i].lossyScale.y,
                    wheelMeshes[i].transform.localPosition.z
                    );
            }
            Debug.DrawRay(wheels[i].transform.position, -wheels[i].transform.up * suspensionRestDist, Color.green, Time.fixedDeltaTime);
        }
    }
}
