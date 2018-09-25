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
    private bool grounded;

    //creates the doubleJumped variable
    private bool doubleJumped;

    //Added from class
    private float moveInput;

    //End of Class Addition

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
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
    }

    private void GetMovementInput()
    {
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
        //TODO: make the character jump
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpHeight)
    }
}