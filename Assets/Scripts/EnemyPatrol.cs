using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed, checkRadius, attackDelay;

    [SerializeField]
    private short enemyHealth;
    private float attackTimer, lastAttack;

    private bool hittingWall, notEdge, playerInRange, moveRight, isAttacking;

    //TODO: TEMPORARILY PUBLIC FOR DEBUGGING
    public bool hasAttacked;

    //TODO: Player Objects to get PlayerController Script
    //GameObject playerObject;
    PlayerController playerController;

    [SerializeField]
    Transform pathCheck;

    [SerializeField]
    Transform edgeCheck;
    
    [SerializeField]
    LayerMask whatIsWall, whatIsPlayer;
    
    Rigidbody2D enemyRigidBody;

    Animator anim;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        enemyRigidBody = GetComponent<Rigidbody2D>();        
        hasAttacked = false;
    }

    private void FixedUpdate()
    {
        PositionChecks();
        Patrol();        
        
    }

    // Update is called once per frame
    void Update () {
        MeleeCheck();
        anim.SetFloat("Speed", Mathf.Abs(enemyRigidBody.velocity.x));
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
            //Get PlayerController from player in order to access; int scoreCounter & SetScoreText()
            playerController = collision.GetComponentInParent<PlayerController>();
            playerController.scoreCounter++;
            playerController.SetScoreText();

            //TODO: Add knock back to enemy

            //Receive damage from player
            enemyHealth--;
            Debug.Log("Bandit Took Damage");

            //Check Enemy Health
            if (enemyHealth == 0)
                Death();
        }
    }

    //------------------------------------------------------------------<<<<--------ENEMY MELEE CHECK-----------
    void MeleeCheck()
    {
        if (playerInRange && !hasAttacked && !isAttacking)
        {
            //TODO: Debug.Log("Delaying....");
            Debug.Log("Player Spotted");
            

            //Activate Attack
            anim.SetTrigger("AttackPlayer");
            hasAttacked = true;

            //Set Enemy Attack State to TRUE
            isAttacking = true;
            if (isAttacking)
            {
                //Freeze Enemy Movement
                while (isAttacking)
                {
                    moveSpeed = 0;
                }
                //Attack Delay Coroutine
                StartCoroutine(WaitAfterHit());
            }           
            //hasAttacked = false;
            //SetAttackDelay();
            //AttackTimer();
        }
        /*
        **TODO: Temporarily disabled to test what occurs without this
        else if (!playerInRange && !hasAttacked && !isAttacking)
        {
            anim.SetBool("AttackPlayer", false);
            Debug.Log("EnemyPatrol.MeleeCheck().else interuption");
        }
        
         */
            
        
    }

    //------------------------------------------------------------------<<<<--------ATTACK DELAY COROUTINE-----------
    IEnumerator WaitAfterHit()
    {
        //TODO: Remove Debug code when debugging is complete
        Debug.Log("Coroutine activated");
        anim.ResetTrigger("AttackPlayer");
        Debug.Log("Bandit Attack Trigger Reset");
        Debug.Log("Initiating Delay...");
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("Delay Successful");
        hasAttacked = false;
        Debug.Log("Bandit's 'hasAttacked' variable = " + hasAttacked);
        isAttacking = false;
        Debug.Log("Now Exiting Coroutine...");
    }

    //Remove Object When Player Kills It
    private void Death()
    {        
        Destroy(gameObject, 0.1f);        
    }

    /***Attemping to Use a Coroutine in place of these methods
    void SetAttackDelay()
    {
        //TODO: Debug.Log("Timer Set");
        Debug.Log("Timer Set");
        lastAttack = Time.time;
        attackTimer = lastAttack + attackDelay;
    }

    void AttackTimer()
    {
        //TODO: Debug.Log("Retrieved when the enemy attacked");
        Debug.Log("Retrieved when the enemy attacked");
        if (hasAttacked && Time.time > attackTimer && playerInRange)
        {
            //TODO: Debug.Log("Delaying....");
            Debug.Log("Delaying....");
            anim.SetBool("AttackPlayer", false);
            hasAttacked = false;
            //MeleeCheck();
        }
    }
    */

}
