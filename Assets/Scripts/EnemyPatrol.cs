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

    private bool hittingWall, notEdge, playerInRange, moveRight;

    //TODO: TEMPORARILY PUBLIC FOR DEBUGGING
    public bool hasAttacked;

    PlayerController player;

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
        player = new PlayerController();
        hasAttacked = false;
    }

    private void FixedUpdate()
    {
        PositionChecks();
        Patrol();        
        MeleeCheck();
        
    }

    // Update is called once per frame
    void Update () {
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

    void MeleeCheck()
    {
        if (playerInRange)
        {
            //TODO: Debug.Log("Delaying....");
            Debug.Log("Player Spotted");
            enemyRigidBody.velocity = new Vector2(0, enemyRigidBody.velocity.y);
            anim.SetBool("AttackPlayer", true);
            hasAttacked = true;
            SetAttackDelay();
            AttackTimer();
        }            
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttackPoint")
        {
            enemyHealth--;
            if (enemyHealth == 0)
                Death();
        }
    }

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
    
    //Remove Object When Player Kills It
    private void Death()
    {
        //TODO: Get Player Score To Increase With Each Kill
        //player.scoreCounter++;
        //player.SetScoreText();
        Destroy(gameObject, 0.2f);        
    }
}
