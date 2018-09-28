using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    private LevelManager levelManager;
    

	// Use this for initialization
	void Start () {
        levelManager = FindObjectOfType<LevelManager>();        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            levelManager.currentCheckpoint = gameObject;
            Debug.Log("You have reached a checkpoint");
        }
    }
}
