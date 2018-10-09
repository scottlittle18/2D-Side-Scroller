using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    private LevelManager levelManager;
    
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Sets the currentCheckpoint to the object this script is attatched to
            levelManager.currentCheckpoint = gameObject;            
        }
    }
}
