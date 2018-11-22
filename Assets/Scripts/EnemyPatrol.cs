using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed, checkRadius, attackDelay, deathDelay;

    [SerializeField]
    private short enemyHealth;

    private bool hittingWall, notEdge, playerInRange, moveRight, isAttacking, hasAttacked;
    
    PlayerController playerController;

    CircleCollider2D enemyAttackPoint;

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
        enemyAttackPoint = GetComponentInChildren<CircleCollider2D>();
        enemyAttackPoint.enabled = false;
    }

    //FixedUpdate
    void FixedUpdate()
    {
        PositionChecks();
        Patrol();        
    }

    // Update is called once per frame
    void Update () {
        MeleeCheck();
        anim.SetFloat("Speed", Mathf.Abs(enemyRigidBody.velocity.x));

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Anim_Bandit_Attack"))
            enemyAttackPoint.enabled = true;
        else
            enemyAttackPoint.enabled = false;
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
            //Get PlayerController from player GameObject in order to access scoreCounter & SetScoreText()
            playerController = collision.GetComponentInParent<PlayerController>();
            playerController.scoreCounter++;
            playerController.SetScoreText();

            //TODO: Add knock back to enemy

            //Receive damage from player
            enemyHealth--;

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
