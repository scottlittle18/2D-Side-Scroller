﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private PlayerController player;
    public GameObject currentCheckpoint;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RespawnPlayer()
    {
        //TODO: Handle player death and check for Respawn
        Debug.Log("Player has Respawned");
        player.myRigidBody.transform.position = currentCheckpoint.transform.position;
    }
}

