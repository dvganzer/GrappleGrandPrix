using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{
    [Header("Jump")]
    public float jumpForce = 5f;
    Vector3 moveDirection;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }

    Rigidbody rb;

    private void Start()
    {
        Debug.Log(isGrounded);
        //Movement
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }
    private void Update()
    {
        //Movement && Jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

    }

}
