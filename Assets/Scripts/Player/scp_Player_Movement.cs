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
    private float horizontalRotation;
    private float verticalRotation;
    [Header("Movement Values")]
    public float speed = 10f;
    public float cameraSpeed = 100f;
    public float bounceUp = 10f;
    public float bounceDown = 10f;

    //References to objects and components
    [HideInInspector] public Rigidbody rb;
    [Header("References")]
    public Transform mainCamera;
    public Transform root;
    public Transform hammy;
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
        //Sets the hamster model's position to the same as this gameobject
        hammy.transform.position = this.transform.position;
    }
    void FixedUpdate()
    {
        //Sets the hamster's position to the same as this gameobject
        root.transform.position = this.transform.position;

        //Stores the direction of the camera as forward and right variables
        Vector3 camF = mainCamera.forward;
        Vector3 camR = mainCamera.right;
        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        //Adds force to the player based on their inputs, speed, and camera direction
        rb.AddForce((camF*vertical + camR*horizontal) * speed, ForceMode.Acceleration);

        //Controls camera rotation
        Vector2 rotation = new Vector2(-verticalRotation, horizontalRotation) * cameraSpeed * Time.deltaTime;
        root.Rotate(rotation);
    }

    //Reads the input values of the roll action and stores them as horizontal and vertical variables
    public void Roll(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;   
        vertical = context.ReadValue<Vector2>().y;  
    }

    //Reads the input values of the look action and stores them as horizontal and vertical variables
    public void Look(InputAction.CallbackContext context)
    {
        horizontalRotation = context.ReadValue<Vector2>().x;   
        verticalRotation = context.ReadValue<Vector2>().y;  
    }

    //If the context is performed while grounded, force is applied to the rigidbody upwards
    //If the context is performed off the ground, force is applied to the rigidbody downwards
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

    //Performing the context increases the rigidbody's drag variable, and cancelling it returns it to default
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

    //Creates a check sphere at the positon of the ground check transform, and returns the IsGrounded boolean
    //if the sphere detects an object on the ground layer
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
    }
}
