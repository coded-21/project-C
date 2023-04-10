using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Moved(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized * moveSpeed;
        if (context.performed)
        {
            Vector3 desiredVel = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
            rb.velocity = desiredVel;
            //rb.AddForce(moveSpeed * moveDirection * Time.fixedDeltaTime, ForceMode.VelocityChange);
            transform.rotation = Quaternion.LookRotation(moveDirection , Vector3.up);
        }

        if (context.canceled)
        {
            Vector3 desiredVel = new Vector3(0, rb.velocity.y, 0);
            rb.velocity = desiredVel;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("jumped");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
