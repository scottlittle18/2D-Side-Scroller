using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    [SerializeField]
    private float accelerationForce, maxSpeed,  jumpHeight, groundCheckRadius, 
        respawnDelay, kickbackPower, kickbackHeight, attackRadius, knockbackTimer;
    [HideInInspector]
    public int scoreCounter;
    private float moveInput, respawnTimer;

    //Jump and Attack Inputs and Release variables
    private bool jumpInput, jumpRelease, attackInput, attackRelease, beingKnockedback;
    private short PlayerHealth;
    
    //Checks which side the player was hit on
    bool hitOnRight;

    //Checkpoint Variable
    [SerializeField]
    private Checkpoint currentCheckpoint;
    public GameObject spawnPoint, HealthMeter;
    
    //Ground Establishment and Detection variables
    [SerializeField]
    private Transform groundCheck, attackPoint;

    [SerializeField]
    private LayerMask whatIsGround;

    AudioSource footsteps;
    [SerializeField]
    Text scoreText;

    private bool grounded, doubleJumped, isDead, allowMoveInput;

    [SerializeField]
    private PhysicsMaterial2D playerMovingPM, playerStoppingPM;
    
    private Animator anim, HealthAnim;    
    
    private Rigidbody2D myRigidBody;

    private SpriteRenderer playerBody;
    
    private Collider2D playerGroundCollider;

    // Use this for initialization
    void Start()
    {
        attackPoint.gameObject.SetActive(false);
        footsteps = GetComponent<AudioSource>();
        playerBody = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerGroundCollider = GetComponent<CapsuleCollider2D>();
        spawnPoint = GameObject.Find("SpawnPoint");
        scoreCounter = 0;
        SetScoreText();
        HealthMeter = GameObject.Find("HealthMeter");
        PlayerHealth = 6;
        HealthAnim = HealthMeter.GetComponent<Animator>();
        UpdateHealth();
        allowMoveInput = true;
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
        if(allowMoveInput && !isDead)
            GetMovementInput();

        CheckForRespawn();
        AudioHandler();

        //----TO UPDATE THE ANIMATOR----
        anim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));
        anim.SetBool("Grounded", grounded);
    }


    private void UpdatePhysicsMaterial()
    {
        if (!grounded && !beingKnockedback)  //<-- TODO: try including '|| beingKnockedback' as a bool to fix knockback issue!!!!!!!!!
            playerMovingPM.friction = 0;            

        if ((Mathf.Abs(moveInput) > 0) && !beingKnockedback)
        {
            playerGroundCollider.sharedMaterial = playerMovingPM;
        }
        else if (!beingKnockedback)
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
        attackInput = Input.GetButtonDown("Fire2");
        attackRelease = Input.GetButtonUp("Fire2");

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

        //Enables Attack
        if (attackInput)
        {
            anim.SetTrigger("Attack");
            attackPoint.gameObject.SetActive(true);
        }
        if (attackRelease)
        {
            anim.ResetTrigger("Attack");
            attackPoint.gameObject.SetActive(false);
        }            

    }


    private void Move()
    {        
        myRigidBody.AddForce(Vector2.right * moveInput * accelerationForce);

        Vector2 clampedVelocity = myRigidBody.velocity;

        clampedVelocity.x = Mathf.Clamp(myRigidBody.velocity.x, -maxSpeed, maxSpeed);

        myRigidBody.velocity = clampedVelocity;

        if (myRigidBody.velocity.x > 0.1)
        {
            transform.localScale = new Vector3(1, 1, 1);            
        }            
        else if (myRigidBody.velocity.x < -0.1)
        {
            transform.localScale = new Vector3(-1, 1, 1);           
        }


    }

    void AudioHandler()
    {
        if (myRigidBody.velocity.x > 0.1 && grounded)
        {
            footsteps.UnPause();
        }
        else if (myRigidBody.velocity.x < -0.1 && grounded)
        {
            footsteps.UnPause();
        }
        else if (myRigidBody.velocity.x == 0 || !grounded)
        {
            footsteps.Pause();
        }
    }
    
    private void Jump()
    {
        AddJumpForce();
        anim.SetFloat("jumpVelocity", myRigidBody.velocity.y);

        //DOUBLE JUMP CHECK
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

    //------------------------------------------------------------------<<<<--------SETS CURRENT CHECKPOINT-----------
    public void SetCurrentCheckpoint(Checkpoint newCurrentCheckpoint)
    {
        
        if (currentCheckpoint == null)
        {            
            currentCheckpoint = newCurrentCheckpoint;
            newCurrentCheckpoint.SetAsActivated(true);
        }            
        else
        {
            currentCheckpoint.SetAsActivated(false);
            currentCheckpoint = newCurrentCheckpoint;
            newCurrentCheckpoint.SetAsActivated(true);
        }
    }

    void UpdateHealth()
    {
        HealthAnim.SetInteger("PlayerHealth", PlayerHealth);
    }

    //-------------------------------------------------------------------<<<<-------TRIGGER CHECKS-----------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Pickups":
                if (collision.name == "coinSilver")
                {
                    scoreCounter++;
                    SetScoreText();
                }
                else if (collision.name == "coinGold")
                {
                    scoreCounter += 2;
                    SetScoreText();
                }
                break;          

            case "Hazards":
                scoreCounter--;
                SetIsDead(true);
                break;

            case "Killzone":
                scoreCounter--;
                SetIsDead(true);
                break;

            case "Ground":
                break;

            case "Untagged":
                break;

            //Default Case Handler
            default:
                break;
        }       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "EnemyBandit":
                PlayerHealth--;
                if (collision.transform.position.x > transform.position.x)
                    hitOnRight = true;
                else if (collision.transform.position.x < transform.position.x)
                    hitOnRight = false;
                Kickback();
                UpdateHealth();
                scoreCounter--;
                if (PlayerHealth == 0)
                    SetIsDead(true);
                break;

            //Default Case Handler
            default:
                break;
        }
    }

    void Kickback()
    {
        allowMoveInput = false;

        if (hitOnRight)
        {
            //myRigidBody.velocity = new Vector2(-kickbackPower, kickbackHeight);
            myRigidBody.AddForce(Vector2.up * kickbackHeight, ForceMode2D.Impulse);
            myRigidBody.AddForce(Vector2.left * kickbackPower, ForceMode2D.Impulse);
            StartCoroutine(KnockbackTimer());
        }
        else if (!hitOnRight)
        {
            //myRigidBody.velocity = new Vector2(kickbackPower, kickbackHeight);
            myRigidBody.AddForce(Vector2.up * kickbackHeight, ForceMode2D.Impulse);
            myRigidBody.AddForce(Vector2.right * kickbackPower, ForceMode2D.Impulse);
            StartCoroutine(KnockbackTimer());
        }       
    }

    IEnumerator KnockbackTimer()
    {
        beingKnockedback = true;
        playerGroundCollider.sharedMaterial.friction = 0;
        yield return new WaitForSeconds(knockbackTimer);
        beingKnockedback = false;
        allowMoveInput = true;
    }

    public void SetScoreText()
    {
        scoreText.text = "Score: " + scoreCounter.ToString();
    }

    public void SetIsDead(bool dead)
    {
        footsteps.Pause();
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
            transform.position = spawnPoint.transform.position;        
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