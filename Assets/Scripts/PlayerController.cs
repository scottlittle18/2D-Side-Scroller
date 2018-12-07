using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Serialized Fields
    //Movement Variables
    [SerializeField]
    private float accelerationForce;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private float respawnDelay;
    [SerializeField]
    private float kickbackPower;
    [SerializeField]
    private float kickbackHeight;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private float knockbackTimer;
    [SerializeField]
    [Tooltip("The Amount of Time the Player is invincible after taking damage")]
    private float timeInvincible;
    [SerializeField]
    private Transform groundCheck, attackPoint;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    [Tooltip("Active Physics Material While Player Is Moving")]
    private PhysicsMaterial2D playerMovingPhysicsMaterial;
    [SerializeField]
    [Tooltip("Active Physics Material While Player Is Stopping")]
    private PhysicsMaterial2D playerStoppingPhysicsMaterial;
    [SerializeField]
    [Tooltip("Active Physics Material While Player Is Being Knocked Back")]
    private PhysicsMaterial2D playerKnockbackPhysicsMaterial;
    #endregion
    #region Non-Serialized Fields
    private Checkpoint currentCheckpoint;
    private int scoreCounter;
    private float moveInput, respawnTimer;
    //Jump and Attack Inputs and Release variables
    private bool jumpInput, jumpRelease, attackInput, attackRelease, beingKnockedback, isAlive, canDoubleJump, canJump;
    private short PlayerHealth;    
    //Checks which side the player was hit on
    private bool hitOnRight;
    private GameObject spawnPoint, HealthMeter;
    private Transform currentCheckpointLocation, spawnPointLocation;
    private AudioSource footsteps;
    private bool grounded, doubleJumped, isDead, allowMoveInput, isDamagable;    
    private Animator anim, HealthAnim;    
    private Rigidbody2D myRigidBody;
    private SpriteRenderer playerBody;    
    private Collider2D playerGroundCollider;
    private Color playerColor;
    private Renderer playerRend;
    #endregion
    #region Properties
    public Checkpoint CurrentCheckpoint
    {
        get
        {
            return currentCheckpoint;
        }
        set
        {
            if (currentCheckpoint == null)
            {
                currentCheckpoint = value;
                currentCheckpoint.IsActivated = true;
            }
            else
            {
                currentCheckpoint.IsActivated = false;
                currentCheckpoint = value;
                currentCheckpoint.IsActivated = true;
            }
        }
    }

    public int ScoreCounter
    {
        get
        {
            return scoreCounter;
        }
        set
        {
            scoreCounter = value;
            SetScoreText();
        }
    }
    #endregion
    #region Enumerators
    /// <summary>
    /// Controls how long the player is invincible for after being hit
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Controls how long the effects of being knocked back last (e.g. Not being able to move)
    /// </summary>
    /// <returns></returns>
    IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(knockbackTimer);
        beingKnockedback = false;
        anim.SetBool("beingKnockedback", beingKnockedback);
        allowMoveInput = true;
    }
    #endregion
    // Use this for initialization
    private void Start()
    {
        InitializePlayer();
    }
    /// <summary>
    /// Gather and Set Necessary Components, GameObjects, Values, and States
    /// </summary>
    private void InitializePlayer()
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
        ScoreCounter = 0;
        PlayerHealth = 6;
        // Game Objects
        spawnPoint = GameObject.Find("SpawnPoint");
        spawnPointLocation = spawnPoint.transform;        
        HealthMeter = GameObject.Find("PlayerHealthMeter");
        HealthAnim = HealthMeter.GetComponent<Animator>();
        UpdateHealth();
        // States
        attackPoint.gameObject.SetActive(false);
        allowMoveInput = true;
        beingKnockedback = false;
        isDamagable = true;
    }
    
    private void FixedUpdate()
    {
        UpdatePhysicsMaterial();
        if(allowMoveInput && !isDead)
            Move();
        CheckIfOnGround();
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
    /// <summary>
    /// Checks whether the player is on the ground
    /// </summary>
    private void CheckIfOnGround()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position,
            groundCheckRadius, whatIsGround);
    }

    // Update is called once per frame
    private void Update()
    {
        if(allowMoveInput && !isDead)
            GetMovementInput();

        CheckForRespawn();
        AudioHandler();

        if (beingKnockedback)
        {
            allowMoveInput = false;
            playerGroundCollider.sharedMaterial = playerKnockbackPhysicsMaterial;
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
            playerMovingPhysicsMaterial.friction = 0;

        if ((Mathf.Abs(moveInput) > 0) && !beingKnockedback)
        {
            playerGroundCollider.sharedMaterial = playerMovingPhysicsMaterial;
        }
        else if (grounded && !beingKnockedback && (Mathf.Abs(moveInput) == 0))
        {
            playerGroundCollider.sharedMaterial = playerStoppingPhysicsMaterial;
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
        if (jumpInput && !grounded && !doubleJumped)
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

    private void AudioHandler()
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
        if (jumpInput && !grounded && !doubleJumped)
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

    private void UpdateHealth()
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
                    ScoreCounter++;
                    //SetScoreText();
                }
                else if (collision.name == "coinGold")
                {
                    ScoreCounter += 2;
                    //SetScoreText();
                }
                break;    
            case "Hazards":
                if (isDamagable)
                {
                    ScoreCounter--;
                    SetIsDead(true);
                }                
                break;
            case "Killzone":
                ScoreCounter--;
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
                    ScoreCounter--;
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
    // TODO: Prototype for a way to handle any kind of enemy attack
   /* void CollisionWithEnemy(Collider2D collision)
    {

    }
    */

    private void Kickback()
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

    private void SetScoreText()
    {
        scoreText.text = "Score: " + scoreCounter.ToString();
    }

    private void SetIsDead(bool dead)
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
        if (CurrentCheckpoint == null)
            transform.position = spawnPointLocation.position;        
        else
        {
            myRigidBody.velocity = Vector2.zero;
            transform.position = currentCheckpointLocation.position;
        }
        //Reset variables for player Respawn
        isDead = false;
        myRigidBody.transform.rotation = Quaternion.identity;
        myRigidBody.freezeRotation = true;
        playerBody.color = Color.white;        
    }
}