using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Moved(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;
        if (context.performed)
        {
            rb.velocity = new Vector3(input.x, rb.velocity.y,input.y);
            transform.rotation = Quaternion.LookRotation(moveDirection , Vector3.up);
        }
        else if (context.canceled)
        {
            rb.velocity = Vector3.zero;
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
