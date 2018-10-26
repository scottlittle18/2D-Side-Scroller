using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    
    BoxCollider2D box;

    int currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && other.tag == "Player")
        {
            SceneManager.LoadScene(currentScene + 1);
        }
    }
}
