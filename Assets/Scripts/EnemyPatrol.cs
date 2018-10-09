using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed, wallCheckRadius;

    public bool hittingWallLeft, hittingWallRight, moveRight;

    [SerializeField]
    private Transform LwallCheck;
    [SerializeField]
    private Transform RwallCheck;
    [SerializeField]
    private LayerMask whatIsWall;

    [SerializeField]
    private Rigidbody2D enemyRigidBody;

    // Use this for initialization
    void Start() {

    }

    private void FixedUpdate()
    {
        hittingWallLeft = Physics2D.OverlapCircle(LwallCheck.position,
            wallCheckRadius, whatIsWall);
        hittingWallRight = Physics2D.OverlapCircle(RwallCheck.position,
            wallCheckRadius, whatIsWall);
        if (hittingWallLeft || hittingWallRight)
            moveRight = !moveRight;
        

        if (moveRight)
            enemyRigidBody.velocity = new Vector2(moveSpeed, enemyRigidBody.velocity.y);
        else
            enemyRigidBody.velocity = new Vector2(-moveSpeed, enemyRigidBody.velocity.y);



    }

    // Update is called once per frame
    void Update () {
		
	}

    
}
