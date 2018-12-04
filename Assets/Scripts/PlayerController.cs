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
        respawnDelay, kickbackPower, kickbackHeight, attackRadius, knockbackTimer, timeInvincible;
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

    private bool grounded, doubleJumped, isDead, allowMoveInput, isDamagable;

    [SerializeField]
    private PhysicsMaterial2D playerMovingPM, playerStoppingPM, playerKnockbackPM;
    
    private Animator anim, HealthAnim;    
    
    private Rigidbody2D myRigidBody;

    private SpriteRenderer playerBody;
    
    private Collider2D playerGroundCollider;

    Color playerColor;

    Renderer playerRend;

    // Use this for initialization
    void Start()
    {
        SetupPlayer();
    }

    //Gather and Set Necessary Components, GameObjects, Values, and States
    void SetupPlayer()
    {
        // Components
        footsteps = GetComponent<AudioSource>();
        playerBody = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerGroundCollider = GetComponent<CapsuleCollider2D>();
        playerRend = GetComponent<Renderer>();
        playerColor = playerBody.color;

        // Values
        scoreCounter = 0;
        SetScoreText();
        PlayerHealth = 6;

        // Game Objects
        spawnPoint = GameObject.Find("SpawnPoint");
        HealthMeter = GameObject.Find("PlayerHealthMeter");
        HealthAnim = HealthMeter.GetComponent<Animator>();
        UpdateHealth();

        // States
        attackPoint.gameObject.SetActive(false);
        allowMoveInput = true;
        beingKnockedback = false;
        isDamagable = true;
    }
    
    void FixedUpdate()
    {
        UpdatePhysicsMaterial();

        if(allowMoveInput && !isDead)
            Move();

        grounded = Physics2D.OverlapCircle(groundCheck.position, 
            groundCheckRadius, whatIsGround);

        anim.SetFloat("jumpVelocity", myRigidBody.velocity.y);
        if (grounded)
        {
            doubleJumped = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Samurai_Attack"))
            attackPoint.gameObject.SetActive(true);
        else
            attackPoint.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(allowMoveInput && !isDead)
            GetMovementInput();

        CheckForRespawn();
        AudioHandler();

        if (beingKnockedback)
        {
            allowMoveInput = false;
            playerGroundCollider.sharedMaterial = playerKnockbackPM;
            anim.SetBool("beingKnockedback", beingKnockedback);
        }

        //----TO UPDATE THE ANIMATOR----
        anim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("jumpVelocity", myRigidBody.velocity.y);
    }


    private void UpdatePhysicsMaterial()
    {
        if (!grounded && !beingKnockedback)
            playerMovingPM.friction = 0;

        if ((Mathf.Abs(moveInput) > 0) && !beingKnockedback)
        {
            playerGroundCollider.sharedMaterial = playerMovingPM;
        }
        else if (grounded && !beingKnockedback && (Mathf.Abs(moveInput) == 0))
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
        }
        if (attackRelease)
        {
            anim.ResetTrigger("Attack");
        }
    }


    private void Move()
    {        
        myRigidBody.AddForce(Vector2.right * moveInput * accelerationForce);

        Vector2 clampedVelocity = myRigidBody.velocity;

        clampedVelocity.x = Mathf.Clamp(myRigidBody.velocity.x, -maxSpeed, maxSpeed);

        myRigidBody.velocity = clampedVelocity;

        //Sprite Flipping
        if (myRigidBody.velocity.x > 0.1)
        {
            transform.localScale = new Vector3(1, 1, 1);            
        }            
        else if (myRigidBody.velocity.x < -0.1)
        {
            transform.localScale = new Vector3(-1, 1, 1);           
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
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

        //DOUBLE JUMP CHECK
        if (jumpInput && !doubleJumped && !grounded)
        {            
            DoubleJump();
        }
    }

    private void AddJumpForce()
    {
        //myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0);
        myRigidBody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }

    private void AddDoubleJumpForce()
    {
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpHeight);
    }

    private void DoubleJump()
    {
        AddDoubleJumpForce();
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
                if (isDamagable)
                {
                    scoreCounter--;
                    SetIsDead(true);
                }                
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
                if (isDamagable)
                {
                    beingKnockedback = true;
                    PlayerHealth--;
                    if (collision.transform.position.x > transform.position.x)
                        hitOnRight = true;
                    else if (collision.transform.position.x < transform.position.x)
                        hitOnRight = false;
                    Kickback();
                    UpdateHealth();
                    scoreCounter--;
                    StartCoroutine(InvincibilityTimer());
                }
                if (PlayerHealth == 0)
                    SetIsDead(true);
                break;

            //Default Case Handler
            default:
                break;
        }
    }

    IEnumerator InvincibilityTimer()
    {
        isDamagable = false;
        Physics2D.IgnoreLayerCollision(9, 14, true);
        Physics2D.IgnoreLayerCollision(9, 13, true);
        playerColor.a = 0.5f;
        playerRend.material.color = playerColor;
        yield return new WaitForSeconds(timeInvincible);
        playerColor.a = 1f;
        playerRend.material.color = playerColor;
        Physics2D.IgnoreLayerCollision(9, 13, true);
        Physics2D.IgnoreLayerCollision(9, 14, false);
        isDamagable = true;
    }

    // TODO: Prototype for a way to handle any kind of enemy attack
   /* void CollisionWithEnemy(Collider2D collision)
    {

    }
    */

    void Kickback()
    {
        if (hitOnRight)
        {
            myRigidBody.AddForce(Vector2.up * kickbackHeight, ForceMode2D.Impulse);
            myRigidBody.AddForce(Vector2.left * kickbackPower, ForceMode2D.Impulse);
        }
        else if (!hitOnRight)
        {
            myRigidBody.AddForce(Vector2.up * kickbackHeight, ForceMode2D.Impulse);
            myRigidBody.AddForce(Vector2.right * kickbackPower, ForceMode2D.Impulse);
        }

        // Start Knockback Timer
        StartCoroutine(KnockbackTimer());
    }

    IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(knockbackTimer);
        beingKnockedback = false;
        anim.SetBool("beingKnockedback", beingKnockedback);
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