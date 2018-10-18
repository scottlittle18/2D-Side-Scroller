using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    
        
    //Start() and Update() are not needed for this script 
    //  it is looking for a collision and doesnt need to update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();            
            player.SetIsDead(true);
            
        }
    }
}
