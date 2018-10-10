using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //creates a variable that refers to the PlayerController
            PlayerController player = collision.GetComponent<PlayerController>();
            //Sends whatever object this script is attached to  to the PlayerController
            player.SetCurrentCheckpoint(this);
        }
    }
}
