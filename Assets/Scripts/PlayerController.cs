using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    [SerializeField]
    private float accelerationForce, maxSpeed,  jumpHeight, groundCheckRadius, respawnDelay;
    private int rotationAmount;
    private float moveInput, respawnTimer, rotationSpeed = 5;
    private bool jumpInput;

    //refresh the Jump button press state
    private bool jumpRelease;

    //Checkpoint Variable
    public Checkpoint currentCheckpoint;
    
    //Ground Establishment and Detection variables
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;

    private bool grounded, doubleJumped, isDead;

    [SerializeField]
    private PhysicsMaterial2D playerMovingPM, playerStoppingPM;

    [SerializeField]
    private Animator anim;
    
    [SerializeField]
    private Rigidbody2D myRigidBody;

    public SpriteRenderer playerBody;

    [SerializeField]
    private Collider2D playerGroundCollider;

    // Use this for initialization
    void Start()
    {
        playerBody = GetComponent<SpriteRenderer>();
    }

    
    void FixedUpdate()
    {
        UpdatePhysicsMaterial();

        if(!isDead)
            Move();
        grounded = Physics2D.OverlapCircle(groundCheck.position, 
            groundCheckRadius, whatIsGround);

        anim.SetFloat("jumpVelocity", myRigidBody.velocity.y);
        if (grounded)
            {
                doubleJumped = false;            
            }
    }


    // Update is called once per frame
    void Update()
    {        
        GetMovementInput();
        CheckForRespawn();

        //Send the player's speed to the animator to let it play the run animation
        anim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));
        anim.SetBool("Grounded", grounded);
    }


    private void UpdatePhysicsMaterial()
    {
        if (!grounded)
            playerMovingPM.friction = 0;

        if (Mathf.Abs(moveInput) > 0)
        {
            playerGroundCollider.sharedMaterial = playerMovingPM;
        }
        else
        {
            playerGroundCollider.sharedMaterial = playerStoppingPM;
        }
    }


    private void GetMovementInput()
    {
        //Movement Variables
        moveInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        jumpRelease = Input.GetButtonUp("Jump");

            //Enables Jumping
            if (jumpInput && grounded)
            {
                Jump();
            }        

            // Enables Double jumping
            if(jumpInput && !grounded && !doubleJumped)
            {
                DoubleJump();
            }        
    }


    private void Move()
    {        
        myRigidBody.AddForce(Vector2.right * moveInput * accelerationForce);

        Vector2 clampedVelocity = myRigidBody.velocity;

        clampedVelocity.x = Mathf.Clamp(myRigidBody.velocity.x, -maxSpeed, maxSpeed);

        myRigidBody.velocity = clampedVelocity;

        if (myRigidBody.velocity.x > 0.1)
            transform.localScale = new Vector3(1, 1, 1);
        else if (myRigidBody.velocity.x < -0.1)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    
    private void Jump()
    {
        AddJumpForce();
        anim.SetFloat("jumpVelocity", myRigidBody.velocity.y);

        //Check for Second jump input to allow Double Jumping
        if (jumpInput && !doubleJumped && !grounded)
        {
            DoubleJump();
        }
    }

    private void AddJumpForce()
    {
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0);
        myRigidBody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }

    private void DoubleJump()
    {
        AddJumpForce();
        doubleJumped = true;
    }

    //----------------SETS CURRENT CHECKPOINT-----------
    public void SetCurrentCheckpoint(Checkpoint newCurrentCheckpoint)
    {            
        if(currentCheckpoint = null)
            currentCheckpoint.SetAsActivated(false);
        else
        {            
            currentCheckpoint = newCurrentCheckpoint;
            newCurrentCheckpoint.SetAsActivated(true);
        }
    }

    public void SetIsDead(bool dead)
    {
        isDead = dead;
        SetRespawnTimer();
    }

    private void SetRespawnTimer()
    {
        respawnTimer = Time.time + respawnDelay;
    }

    private void CheckForRespawn()
    {
        if (isDead)
        {
            anim.Play("Anim_Samurai_Death", 0);
            myRigidBody.freezeRotation = false;
        }
             

        if (isDead && Time.time > respawnTimer)
        {
            playerBody.color = Color.clear;
            SetRespawnTimer();

            Respawn();            
        }
    }

    private void Respawn()
    {       
            if (currentCheckpoint == null)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
            {
                myRigidBody.velocity = Vector2.zero;
                transform.position = currentCheckpoint.transform.position;
            }

        //Reset variables for player Respawn
        isDead = false;
        myRigidBody.transform.rotation = Quaternion.identity;
        myRigidBody.freezeRotation = true;
        playerBody.color = Color.white;        
    }
}