using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed, jumpHeight, checkRadius;

    public bool hittingWall, notEdge, canJump, moveRight, jumpRight;

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform jumpCheck;

    [SerializeField]
    private Transform edgeCheck;
    
    [SerializeField]
    private LayerMask whatIsWall;

    [SerializeField]
    private Rigidbody2D enemyRigidBody;

    // Use this for initialization
    void Start() {

    }

    private void FixedUpdate()
    {
        hittingWall = Physics2D.OverlapCircle(wallCheck.position,
            checkRadius, whatIsWall);
        
        notEdge = Physics2D.OverlapCircle(edgeCheck.position,
            checkRadius, whatIsWall);

        //canJump = Physics2D.OverlapCircle(jumpCheck.position,
            //checkRadius, whatIsWall);
        


        if ((hittingWall || notEdge) && !canJump)
            moveRight = !moveRight;
        //else if ((canJump || !notEdge) && !hittingWall)
          //  jumpRight = !jumpRight;


        if (moveRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            enemyRigidBody.velocity = new Vector2(moveSpeed, enemyRigidBody.velocity.y);

           /* if (jumpRight && !notEdge)
            {
                do
                {
                    enemyRigidBody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                }
                while (jumpRight && !notEdge);
            }*/
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            enemyRigidBody.velocity = new Vector2(-moveSpeed, enemyRigidBody.velocity.y);

            /*if (jumpRight && !notEdge)
            {
                do
                {
                    enemyRigidBody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                }
                while (jumpRight && !notEdge);
            }
            */
        }

            





    }

    

    // Update is called once per frame
    void Update () {
		
	}

    
}
