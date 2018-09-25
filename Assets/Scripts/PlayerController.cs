using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed, jumpHeight, groundCheckRadius;

    //Ground variables
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private bool grounded;

    //creates the doubleJumped variable
    private bool doubleJumped;

    //Added from class
        
    //End of Class Addition

    //Makes it so that you dont have to Prefix it with GetComponent2d<>
    public Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        // Debug.Log("This is Start");
    }

    //Ground Check
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    // Update is called once per frame
    void Update()
    {
        //resets the doubleJump variable when the player touches the ground
        if (grounded)
            doubleJumped = false;

        //Enables Jump
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            myRigidBody.velocity = new Vector2(0, jumpHeight);
        }

        //Allows Double Jumping
        if (Input.GetKeyDown(KeyCode.W) && !doubleJumped && !grounded)
        {
            //GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpHeight);
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpHeight);
            doubleJumped = true;
        }

        //Controls Player's horizontal forward movement
        if (Input.GetKey (KeyCode.D))
        {
            //GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
            myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
        }

        //Controls Player's horizontal backward movement
        if (Input.GetKey (KeyCode.A))
        {
           // GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, 0);
            myRigidBody.velocity = new Vector2(-moveSpeed, myRigidBody.velocity.y);
        }
    }
}
