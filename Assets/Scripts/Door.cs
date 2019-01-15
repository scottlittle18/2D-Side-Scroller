﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private List<GameObject> enemyBandits = new List<GameObject>();
    private int currentScene;

    public List<GameObject> EnemyBandits
    {
        get
        {
            return enemyBandits;
        }
        set
        {
            enemyBandits = value;
        }
    }
    
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;

        foreach (GameObject Enemy in GameObject.FindGameObjectsWithTag("EnemyBandit"))
        {
            EnemyBandits.Add(Enemy);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((Input.GetAxis("Vertical") > 0) && other.tag == "Player")
        {
            HaltEnemyMovement();
            PlayerPrefs.SetInt("CurrentPlayerHealth", other.GetComponent<PlayerController>().CurrentPlayerHealth);
            SceneManager.LoadScene(currentScene + 1);
        }
    }

    /// <summary>
    /// Stop enemy movement while the next scene is loading
    /// </summary>
    private void HaltEnemyMovement()
    {
        foreach (GameObject Enemy in EnemyBandits)
        {
            //EnemyBandits.Remove(Enemy);
            Enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
