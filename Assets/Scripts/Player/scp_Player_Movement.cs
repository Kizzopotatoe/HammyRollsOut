using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class scp_Player_Movement : MonoBehaviour
{
    //Floats storing movement values
    private float horizontal;
    private float vertical;
    public float speed = 10f;
    public float bounceUp = 10f;
    public float bounceDown = 10f;

    //References to objects and components
    public Rigidbody rb;
    public Transform mainCamera;
    public Transform hamster;
    public Transform groundCheck;
    public LayerMask groundLayer;


    // Start is called before the first frame update
    void Start()
    {
        //Stores the player's rigidbody
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Sets the hamsters  position to the same as this gameobject
        hamster.transform.position = this.transform.position;

        Vector3 camF = mainCamera.forward;
        Vector3 camR = mainCamera.right;
        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        //Adds force in a direction based on the player's inputs and speed
        rb.AddForce((camF*vertical + camR*horizontal) * speed);
    }

    public void Roll(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;   
        vertical = context.ReadValue<Vector2>().y;  
    }

    public void Bounce(InputAction.CallbackContext context)
    {
        if(context.performed && IsGrounded())
        {
            rb.AddForce(Vector3.up * bounceUp, ForceMode.Impulse);
        }
        if(context.performed && !IsGrounded())
        {
            rb.AddForce(Vector3.down * bounceDown, ForceMode.Impulse);
        }
    }

    public void Brake(InputAction.CallbackContext context)
    {
        if(context.performed && IsGrounded())
        {
            rb.drag = 10f;
        }
        if(context.canceled)
        {
            rb.drag = 0.5f;
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
    }
}
