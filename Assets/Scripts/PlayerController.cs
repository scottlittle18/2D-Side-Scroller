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

    // Use this for initialization
    void Start()
    {

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
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpHeight);
        }

        //Allows Double Jumping
        if (Input.GetKeyDown(KeyCode.W) && !doubleJumped && !grounded)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpHeight);
            doubleJumped = true;
        }

        //Controls Player's horizontal forward movement
        if (Input.GetKey (KeyCode.D))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
        }

        //Controls Player's horizontal backward movement
        if (Input.GetKey (KeyCode.A))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, 0);
        }
    }
}
