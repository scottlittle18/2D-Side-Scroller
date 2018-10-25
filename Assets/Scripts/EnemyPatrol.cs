using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed, checkRadius, attackDelay;

    private float attackTimer;

    private bool hittingWall, notEdge, playerInRange, moveRight, hasAttacked;

    [SerializeField]
    private Transform pathCheck;

    [SerializeField]
    private Transform edgeCheck;
    
    [SerializeField]
    private LayerMask whatIsWall, whatIsPlayer;

    [SerializeField]
    private Rigidbody2D enemyRigidBody;

    Animator anim;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        PositionChecks();

        MeleeCheck();

        Patrol();        
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

    //Checks if the player is within MELEE ATTACK range of the Enemy
    private void MeleeCheck()
    {
        if (playerInRange && !hasAttacked)
        {
            anim.SetBool("AttackPlayer", true);
            hasAttacked = true;
            MeleeAttackDelay();
        }            
        else if (!playerInRange)
            anim.SetBool("AttackPlayer", false);
    }

    private void ResetMeleeAttackTimer()
    {
        attackTimer = Time.time + attackDelay;
    }

    private void MeleeAttackDelay()
    {
        if(hasAttacked && Time.time > attackTimer)
        {
            hasAttacked = false;
            ResetMeleeAttackTimer();
        }
    }

    // Update is called once per frame
    void Update () {
        anim.SetFloat("Speed", Mathf.Abs(enemyRigidBody.velocity.x));
    }

    
}
