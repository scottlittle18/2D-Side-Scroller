using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed, wallCheckRadius;

    private bool hittingWall, moveRight;

    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private LayerMask whatIsWall;

    [SerializeField]
    private Rigidbody2D enemyRigidBody;

	// Use this for initialization
	void Start () {
		
	}

    private void FixedUpdate()
    {
        Patrol();

        hittingWall = Physics2D.OverlapCircle(wallCheck.position, 
            wallCheckRadius, whatIsWall);

        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Patrol()
    {
        if (hittingWall)
            moveRight = !moveRight;

        if (moveRight)
            enemyRigidBody.velocity = new Vector2(moveSpeed, enemyRigidBody.velocity.y);
        else
            enemyRigidBody.velocity = new Vector2(-moveSpeed, enemyRigidBody.velocity.y);
    }
}
