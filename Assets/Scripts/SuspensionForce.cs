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

    [Header("Car Stats")]
    [SerializeField] private float suspensionRestDist;
    [SerializeField] private float springPower;
    [SerializeField] private float springDamper;
    [SerializeField] private float wheelRadius;
    [SerializeField] private float wheelMass;
    [Range(0f,1f)]
    [SerializeField] private float wheelGripFactor;

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

                // traction force
                Vector3 steeringDir = wheels[i].transform.right;

                float wheelSidewaysVelocity = Vector3.Dot(carRb.GetPointVelocity(wheels[i].position), steeringDir);
                float desiredVelChange = -wheelSidewaysVelocity * wheelGripFactor;
                float desiredAcc = desiredVelChange / Time.fixedDeltaTime;
                float tractionForce = desiredAcc * wheelMass;

                carRb.AddForceAtPosition(tractionForce * steeringDir, wheels[i].transform.position);
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
