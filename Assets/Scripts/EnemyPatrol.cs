using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed, checkRadius, attackDelay, deathDelay, knockbackPower, knockbackHeight, knockbackTime;

    public short enemyHealth;

    private bool hittingWall, notEdge, playerInRange, moveRight, 
        isAttacking, hasAttacked, attackedOnRight, canMove, isDamagable, beingKnockedBack;
    
    PlayerController playerController;

    EnemyHealth EnemyHealthController;

    CircleCollider2D enemyAttackPoint;

    [SerializeField]
    Transform pathCheck;

    [SerializeField]
    Transform edgeCheck;
    
    [SerializeField]
    LayerMask whatIsWall, whatIsPlayer;
    
    Rigidbody2D enemyRigidBody;

    Animator anim;

    [SerializeField]
    PhysicsMaterial2D knockbackPM, patrolPM;

    Collider2D enemyPrimaryCollision;

    // Use this for initialization
    void Start() {
        SetupObject();
    }

    //Get Necessary Components and Set Starting States
    void SetupObject()
    {
        //Gather Necessary Components
        anim = GetComponent<Animator>();
        enemyRigidBody = GetComponent<Rigidbody2D>();        
        enemyAttackPoint = GetComponentInChildren<CircleCollider2D>();
        enemyPrimaryCollision = GetComponent<CapsuleCollider2D>();
        EnemyHealthController = GetComponentInChildren<EnemyHealth>();

        //Set Starting States
        EnemyHealthController.UpdateHealth(enemyHealth);
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
    void Update () {
        MeleeCheck();
        anim.SetFloat("Speed", Mathf.Abs(enemyRigidBody.velocity.x));

        UpdatePM();

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Anim_Bandit_Attack"))
            enemyAttackPoint.enabled = true;
        else
            enemyAttackPoint.enabled = false;
    }

    void UpdatePM()
    {
        if (beingKnockedBack)
            enemyPrimaryCollision.sharedMaterial = knockbackPM;
        else
            enemyPrimaryCollision.sharedMaterial = patrolPM;
    }

    //Handle movement of Enemies to let them 'patrol' the area
    private void Patrol()
    {
        if (hittingWall || notEdge)
            moveRight = !moveRight;

        if (moveRight)
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

    //Check position of the enemy based on the associated Transform GameObjects
    private void PositionChecks()
    {
        //WALL CHECK
        hittingWall = Physics2D.OverlapCircle(pathCheck.position,
            checkRadius, whatIsWall);

        //EDGE CHECK
        notEdge = Physics2D.OverlapCircle(edgeCheck.position,
            checkRadius, whatIsWall);

        //PLAYER CHECK
        playerInRange = Physics2D.OverlapCircle(pathCheck.position,
            checkRadius, whatIsPlayer);
    }

    //------------------------------------------------------------------<<<<--------ENEMY COLLISION HANDLER-----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Detects melee attack from player
        if (collision.tag == "PlayerAttackPoint")
        {
            //Set current state to beingKnockedBack
            beingKnockedBack = true;

            

            //Keeps the Enemy from taking damage & adding to the player score multiple times from a single attack
            if (isDamagable)
            {
                //Get PlayerController from player GameObject in order to access scoreCounter & SetScoreText()
                playerController = collision.GetComponentInParent<PlayerController>();
                playerController.scoreCounter++;
                playerController.SetScoreText();

                //Receive damage from player
                enemyHealth--;
                EnemyHealthController.UpdateHealth(enemyHealth);
            }

            isDamagable = false;

            //TODO: Add knock back to enemy
            if (collision.transform.position.x > transform.position.x)
            {
                attackedOnRight = true;
                EnemyKnockback();
                StartCoroutine(KnockbackTime());
            }                
            else if (collision.transform.position.x < transform.position.x)
            {
                attackedOnRight = false;
                EnemyKnockback();
                StartCoroutine(KnockbackTime());
            }

            //Check Enemy Health
            if (enemyHealth == 0)
                Death();
        }
    }

    void EnemyKnockback()
    {
        canMove = false;

        
        if (attackedOnRight)
        {
            enemyRigidBody.velocity = new Vector2(-knockbackPower, knockbackHeight);
        }
        else if (!attackedOnRight)
        {
            enemyRigidBody.velocity = new Vector2(knockbackPower, knockbackHeight);
        }
    }

    IEnumerator KnockbackTime()
    {
        yield return new WaitForSeconds(knockbackTime);
        beingKnockedBack = false;
        isDamagable = true;
        canMove = true;
    }
    //------------------------------------------------------------------<<<<--------ENEMY MELEE CHECK-----------
    void MeleeCheck()
    {
        if (playerInRange && !hasAttacked && !isAttacking)
        {
            //Trigger Attack Animation
            anim.SetTrigger("AttackPlayer");
            
            //Set Enemy Attack State to TRUE
            hasAttacked = true;
            isAttacking = true;

            //Start Coroutine if isAttacking == true regardless of the player's distance
            if (isAttacking && (playerInRange || !playerInRange))
            {
                //Attack Delay Coroutine
                StartCoroutine(DelayAttack());
            }
        }  
    }

    //------------------------------------------------------------------<<<<--------ATTACK DELAY COROUTINE-----------
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


    //Remove Object When Player Kills It
    private void Death()
    {        
        Destroy(gameObject, deathDelay);        
    }   
}
