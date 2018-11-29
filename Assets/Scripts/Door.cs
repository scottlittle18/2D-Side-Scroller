using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    
    BoxCollider2D box;

    //GameObject enemy;
    GameObject[] EnemyBandits;

    int currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        EnemyBandits = GameObject.FindGameObjectsWithTag("EnemyBandit");
        
    }

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && other.tag == "Player")
        {
            foreach (GameObject Enemy in EnemyBandits)
            {
                Enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            }
            SceneManager.LoadScene(currentScene + 1);
        }
    }
}
