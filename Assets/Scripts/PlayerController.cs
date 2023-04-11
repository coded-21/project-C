using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;
    [Range(0,0.8f)] [SerializeField] float frictionValue;
    [SerializeField] PhysicMaterial playerPhysicsMat;
    private Vector2 moveInput;
    private bool jumpPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 flatVelocity = new Vector3(moveInput.x, 0, moveInput.y).normalized * moveSpeed;
        Vector3 desiredVelocity = new Vector3(flatVelocity.x, rb.velocity.y, flatVelocity.z);
        rb.velocity = desiredVelocity;

        if (jumpPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpPressed = false;
        }
    }

    public void Moved(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
            Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        if (context.canceled)
        {
            moveInput = new Vector2(0, 0);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpPressed = true;
        }
    }
}
