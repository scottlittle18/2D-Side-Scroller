using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    public float moveSpeed, jumpHeight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpHeight);
        }
		
        if (Input.GetKeyDown(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
        }

        if (Input.GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, 0);
	}
}
