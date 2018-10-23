using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed, jumpHeight, checkRadius;

    private bool hittingWall, notEdge, playerInRange, canJump, moveRight, jumpRight;

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
        hittingWall = Physics2D.OverlapCircle(pathCheck.position,
            checkRadius, whatIsWall);
        
        notEdge = Physics2D.OverlapCircle(edgeCheck.position,
            checkRadius, whatIsWall);

        playerInRange = Physics2D.OverlapCircle(pathCheck.position, 
            checkRadius, whatIsPlayer);

        if (hittingWall || notEdge)
            moveRight = !moveRight;

        if (playerInRange)
            anim.SetBool("AttackPlayer", true);
        else if (!playerInRange)
            anim.SetBool("AttackPlayer", false);

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

    

    // Update is called once per frame
    void Update () {
        anim.SetFloat("Speed", Mathf.Abs(enemyRigidBody.velocity.x));
    }

    
}
