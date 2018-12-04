using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    //Declare variables for the different states of health
    Transform fullHealth, halfHealth, noHealth;

    EnemyPatrol EnemyController;
    short health;

	// Use this for initialization
	void Start () {
        fullHealth = transform.GetChild(0);
        halfHealth = transform.GetChild(1);
        noHealth = transform.GetChild(2);

        EnemyController = GetComponentInParent<EnemyPatrol>();
        health = EnemyController.enemyHealth;

        UpdateHealthSprite();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void UpdateHealth(short currentHealth)
    {
        health = currentHealth;
        UpdateHealthSprite();
    }

    void UpdateHealthSprite()
    {
        if (health > 1)
        {
            fullHealth.gameObject.SetActive(true);
            halfHealth.gameObject.SetActive(false);
            noHealth.gameObject.SetActive(false);
        }
        else if (health == 1)
        {
            fullHealth.gameObject.SetActive(false);
            halfHealth.gameObject.SetActive(true);
            noHealth.gameObject.SetActive(false);
        }
        else if (health == 0)
        {
            fullHealth.gameObject.SetActive(false);
            halfHealth.gameObject.SetActive(false);
            noHealth.gameObject.SetActive(true);
        }
    }
}
