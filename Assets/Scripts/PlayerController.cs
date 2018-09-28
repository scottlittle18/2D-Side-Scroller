using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    [SerializeField]
    private float moveSpeed, jumpHeight, groundCheckRadius;

    private float moveInput;
    private bool jumpInput;

    //TODO: This is a test variable used to refresh the Jump button press state
    private bool jumpRelease;

    //Ground Establishment and Detection variables
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;

    //TODO: AFTER-DEBUG Make 'grounded' && 'doubleJumped' variables NON-Serialized
    //Temporarily Visible In Editor for Debugging
    [SerializeField]
    private bool grounded, doubleJumped;
    
    //Creates a new Rigidbody2D object for player
    [SerializeField]
    private Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        
    }

    //Ground Check
    void FixedUpdate()
    {
        Move();
        grounded = Physics2D.OverlapCircle(groundCheck.position, 
            groundCheckRadius, whatIsGround);
        
        if (grounded)
        {
            doubleJumped = false;
            Debug.Log("You are Grounded and Double Jump is now false");
        }
    }

    // Update is called once per frame
    void Update()
    {
              
        GetMovementInput();
    }

    

    private void GetMovementInput()
    {
        jumpInput = Input.GetButtonDown("Jump");// <--- THIS FUCKING CUNT MEISTER!!!!

        jumpRelease = Input.GetButtonUp("Jump");
        //Enables Jump
        if (jumpInput && grounded)
        {
            Jump();

        }
        

        // SUPPOSED to enable double jump
        if(jumpInput && !grounded && !doubleJumped)
        {
            DoubleJump();
        }
        //doubleJumped = false;
        //Used for detecting and directing Left/Right movement
        moveInput = Input.GetAxis("Horizontal");
    }

    private void Move()
    {
        //Don't use transform.Translate since this is a physics object
        //transform.Translate(moveSpeed, jumpHeight, 0);

        myRigidBody.velocity = new Vector2(moveInput * moveSpeed, myRigidBody.velocity.y);
    }

    private void Jump()
    {
        //TODO: make the character JUMP
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpHeight);        
        if (jumpInput && !doubleJumped && !grounded)
        {
            DoubleJump();                       
        }
    }

    private void DoubleJump()
    {
        //TODO: make character DOUBLE-JUMP
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpHeight);
        Debug.Log("You have Double Jumped");
        doubleJumped = true;
    }
}