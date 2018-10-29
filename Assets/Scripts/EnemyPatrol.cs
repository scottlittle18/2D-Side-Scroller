using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed, checkRadius, attackDelay;

    private float attackTimer, lastAttack;

    private bool hittingWall, notEdge, playerInRange, moveRight;

    //TODO: TEMPORARILY PUBLIC FOR DEBUGGING
    public bool hasAttacked;

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

    void MeleeCheck()
    {
        if (playerInRange)
        {
            lastAttack = Time.time;
            Debug.Log("Player Spotted");
            enemyRigidBody.velocity = new Vector2(0, enemyRigidBody.velocity.y);
            anim.SetBool("AttackPlayer", true);
            hasAttacked = true;
            //SetAttackDelay();
            AttackTimer();
        }            
        else if (!playerInRange)
        {
            anim.SetBool("AttackPlayer", false);
        }
            
    }

    void SetAttackDelay()
    {
        attackTimer = lastAttack + attackDelay;
    }

    void AttackTimer()
    {
        if (hasAttacked && Time.time > lastAttack + attackDelay)
        {
            Debug.Log("Timer is called");
            hasAttacked = false;
            //MeleeCheck();
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


    
    
}
