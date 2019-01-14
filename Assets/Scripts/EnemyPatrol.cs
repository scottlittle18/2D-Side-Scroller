using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    [Tooltip("Enemy Movement Speed")]
    private float moveSpeed;
    [SerializeField]
    [Tooltip("Maximum distance from enemy that objects will be detected by the EdgeCheck and PathCheck game objects")]
    private float checkRadius;
    [SerializeField]
    [Tooltip("Adjusts amount of time between attacks")]
    private float attackDelay;
    [SerializeField]
    [Tooltip("Adjusts amount of time between when the enemy dies and when it's removed from the scene")]
    private float deathDelay;
    [SerializeField]
    [Tooltip("Adjusts how far the enemy is knocked back when being attacked")]
    private float knockbackPower;
    [SerializeField]
    [Tooltip("Adjusts how high the enemy is knocked up when being attacked")]
    private float knockbackHeight;
    [SerializeField]
    [Tooltip("Adjusts delay between when the enemy was hit and when it can move again")]
    private float knockbackTime;
    [SerializeField]
    [Tooltip("This is the Game Object that will check what is in the enemy's path")]
    private Transform pathCheck;
    [SerializeField]
    [Tooltip("This is the Game Object that will check if the enemy is at an edge")]
    private Transform edgeCheck;    
    [SerializeField]
    private LayerMask whatIsWall, whatIsPlayer;
    [SerializeField]
    [Tooltip("Physics Material that will be active while being knocked back")]
    private PhysicsMaterial2D knockbackPhysicsMaterial;
    [SerializeField]
    [Tooltip("Physics Material that will be active while patrolling")]
    private PhysicsMaterial2D patrolPhysicsMaterial;
    [SerializeField]
    [Tooltip("Total amount of health")]
    private short enemyHealth;
    [SerializeField]
    private AudioSource AudioSFX;
    [SerializeField]
    private AudioClip attackSound, barkSound;
    #endregion
    #region Non-Serialized Fields
    private bool isHittingWall, isAtEdge, isPlayerInRange, isMovingRight, 
        isAttacking, hasAttacked, attackedOnRight, canMove, isDamagable, beingKnockedBack;    
    private PlayerController playerController;
    private EnemyHealth EnemyHealthController;
    private CircleCollider2D enemyAttackPoint;    
    private Rigidbody2D enemyRigidBody;
    private Animator anim;
    private Collider2D enemyPrimaryCollision;
    private Door doorScript;
    private ScoreCounter scoreCounter;
    #endregion
    #region Enumerators
    IEnumerator KnockbackTime()
    {
        yield return new WaitForSeconds(knockbackTime);
        beingKnockedBack = false;
        isDamagable = true;
        canMove = true;
    }
    
    IEnumerator DelayAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Anim_Bandit_Attack"))
            enemyRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX; // Stop Enemy Movement        
        yield return new WaitForSeconds(attackDelay); //Delay Next Attack
        anim.ResetTrigger("AttackPlayer"); // Reset Attack Trigger
        //Reset Attack States to False
        hasAttacked = false;
        isAttacking = false;
    }
    #endregion
    // Use this for initialization
    void Start()
    {
        SetupObject();
    }
    /// <summary>
    /// Gather and Set Necessary Components, GameObjects, Values, and States
    /// </summary>
    void SetupObject()
    {
        // Components
        anim = GetComponent<Animator>();
        enemyRigidBody = GetComponent<Rigidbody2D>();        
        enemyPrimaryCollision = GetComponent<CapsuleCollider2D>();
        enemyAttackPoint = GetComponentInChildren<CircleCollider2D>();
        EnemyHealthController = GetComponentInChildren<EnemyHealth>();
        doorScript = GameObject.FindGameObjectWithTag("Door").GetComponent<Door>();
        scoreCounter = FindObjectOfType<ScoreCounter>();
        //Values
        EnemyHealthController.CurrentEnemyHealth = enemyHealth;
        // States
        hasAttacked = false;
        enemyAttackPoint.enabled = false;
        canMove = true;
        isDamagable = true;
        beingKnockedBack = false;
    }

    //FixedUpdate
    void FixedUpdate()
    {
        PositionChecks();
        if(canMove)
            Patrol();     
    }

    // Update is called once per frame
    void Update ()
    {
        MeleeCheck();
        anim.SetFloat("Speed", Mathf.Abs(enemyRigidBody.velocity.x));
        UpdatePhysicsMaterial();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Anim_Bandit_Attack"))
            enemyAttackPoint.enabled = true;
        else
            enemyAttackPoint.enabled = false;
    }
    /// <summary>
    /// Updates the current Physics Material
    /// </summary>
    void UpdatePhysicsMaterial()
    {
        if (beingKnockedBack)
            enemyPrimaryCollision.sharedMaterial = knockbackPhysicsMaterial;
        else
            enemyPrimaryCollision.sharedMaterial = patrolPhysicsMaterial;
    }

    //Handle movement of Enemies to let them 'patrol' the area
    private void Patrol()
    {
        if (isHittingWall || isAtEdge)
            isMovingRight = !isMovingRight;
        if (isMovingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            enemyRigidBody.velocity = new Vector2(moveSpeed, enemyRigidBody.velocity.y);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            enemyRigidBody.velocity = new Vector2(-moveSpeed, enemyRigidBody.velocity.y);
        }
    }

    //Check position of the enemy based on the associated Transform.GameObjects
    private void PositionChecks()
    {
        //WALL CHECK
        isHittingWall = Physics2D.OverlapCircle(pathCheck.position,
            checkRadius, whatIsWall);
        //EDGE CHECK
        isAtEdge = Physics2D.OverlapCircle(edgeCheck.position,
            checkRadius, whatIsWall);
        //PLAYER CHECK
        isPlayerInRange = Physics2D.OverlapCircle(pathCheck.position,
            checkRadius, whatIsPlayer);
    }

    //------------------------------------------------------------------<<<<--------ENEMY COLLISION HANDLER-----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Detects melee attack from player
        if (collision.tag == "PlayerAttackPoint")
        {
            AudioSFX.PlayOneShot(barkSound);
            //Set current state to beingKnockedBack
            beingKnockedBack = true;
            canMove = false;
            //Keeps the Enemy from taking damage & adding to the player score multiple times from a single attack
            if (isDamagable)
            {
                //Get PlayerController from player GameObject in order to access scoreCounter & SetScoreText()
                playerController = collision.GetComponentInParent<PlayerController>();
                scoreCounter.ScoreCountKeeper++;
                //Receive damage from player
                enemyHealth--;
                EnemyHealthController.CurrentEnemyHealth = enemyHealth;
            }
            isDamagable = false;
            EnemyKnockbackDirection(collision);
            EnemyKnockback();
            StartCoroutine(KnockbackTime());
            CheckEnemyHealth();
        }
    }

    private void CheckEnemyHealth()
    {
        if (enemyHealth == 0)
            Death();
    }

    /// <summary>
    ///  Determines which direction to knockback the enemy
    /// </summary>
    /// <param name="collision"></param>
    void EnemyKnockbackDirection(Collider2D collision)
    {
        if (collision.transform.position.x > transform.position.x)
        {
            attackedOnRight = true;
        }
        else if (collision.transform.position.x < transform.position.x)
        {
            attackedOnRight = false;
        }
    }

    /// <summary>
    /// Applies Knockback Force
    /// </summary>
    void EnemyKnockback()
    {
        if (attackedOnRight)
        {
            enemyRigidBody.velocity = new Vector2(-knockbackPower, knockbackHeight);
        }
        else if (!attackedOnRight)
        {
            enemyRigidBody.velocity = new Vector2(knockbackPower, knockbackHeight);
        }
    }

    /// <summary>
    /// Checks whether the player is close enough to attack
    /// </summary>
    void MeleeCheck()
    {
        if (isPlayerInRange && !hasAttacked && !isAttacking)
        {
            //Trigger Attack Animation
            anim.SetTrigger("AttackPlayer");
            AudioSFX.PlayOneShot(attackSound);
            
            //Set Enemy Attack State to TRUE
            hasAttacked = true;
            isAttacking = true;

            //Start Coroutine if isAttacking == true regardless of the player's distance
            if (isAttacking && (isPlayerInRange || !isPlayerInRange))
            {
                //Attack Delay Coroutine
                StartCoroutine(DelayAttack());
            }
        }  
    }


    /// <summary>
    /// Remove Object When Player Kills It
    /// </summary>
    private void Death()
    {
        doorScript.EnemyBandits.Remove(gameObject);
        Destroy(gameObject, deathDelay);        
    }   
}
