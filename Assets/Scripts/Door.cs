using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private BoxCollider2D box;
    private GameObject[] EnemyBandits;
    private int currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        EnemyBandits = GameObject.FindGameObjectsWithTag("EnemyBandit");        
    }

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((Input.GetAxis("Vertical") > 0) && other.tag == "Player")
        {
            HaltEnemyMovement();
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
            Enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
