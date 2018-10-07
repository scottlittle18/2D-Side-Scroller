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

    //refresh the Jump button press state
    private bool jumpRelease;
    
    //Ground Establishment and Detection variables
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;

    private bool grounded, doubleJumped;

    [SerializeField]
    private Animator anim;
    
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
            }
    }

    // Update is called once per frame
    void Update()
    {              
        GetMovementInput();
        anim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));

    }

    private void GetMovementInput()
    {
        //Movement Variables
        moveInput = Input.GetAxis("Horizontal");
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
    }

    private void Move()
    {
        myRigidBody.velocity = new Vector2(moveInput * moveSpeed, myRigidBody.velocity.y);
        
    }

    private void Jump()
    {        
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpHeight);
        
            //Check for Second jump input to allow Double Jumping
            if (jumpInput && !doubleJumped && !grounded)
            {
                DoubleJump();                       
            }
    }

    private void DoubleJump()
    {        
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpHeight);        
        doubleJumped = true;
    }
}