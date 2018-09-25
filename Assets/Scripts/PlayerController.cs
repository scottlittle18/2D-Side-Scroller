using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, jumpHeight, groundCheckRadius;

    //Ground variables
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;

    //TODO: AFTER-DEBUG Make 'grounded' variable NON-Serialized
    //Visible In Editor for Debugging
    [SerializeField]
    private bool grounded;

    //creates the doubleJumped variable
    private bool doubleJumped;

    //Added from class
    private float moveInput;
    //Creates a new Rigidbody2D object for player
    [SerializeField]
    private Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        // Debug.Log("This is Start");
    }

    //Ground Check
    void FixedUpdate()
    {
        Move();

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    // Update is called once per frame
    void Update()
    {
        if (grounded)
            doubleJumped = false;        
        GetMovementInput();
    }

    private void GetMovementInput()
    {
        
        //resets the doubleJump variable when the player touches the ground
        

        //Enables Jump
        if (Input.GetButton("Jump") && grounded)
        {
            Jump();
        }
        
        if (Input.GetButton("Jump") && !doubleJumped && !grounded)
        {
            DoubleJump();
            //myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpHeight);            
        }

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
    }

    private void DoubleJump()
    {
        //TODO: make character DOUBLE-JUMP
        Jump();
        doubleJumped = true;
    }
}