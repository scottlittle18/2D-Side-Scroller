using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Used to display the current health of the enemy
/// by managing which of the EnemyHealthMeter object's 
/// children are visible and when
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    private Transform fullHealth, halfHealth, noHealth;

    private EnemyPatrol EnemyController;
    private short currentEnemyHealth;

    private void Awake()
    {
        //Index references for the children of the EnemyHealthMeter
        fullHealth = transform.GetChild(0);
        halfHealth = transform.GetChild(1);
        noHealth = transform.GetChild(2);
        
    }

    // Use this for initialization
    private void Start ()
    {

        UpdateHealthSprite();
	}

    public short CurrentEnemyHealth
    {
        get
        {
            return currentEnemyHealth;
        }
        set
        {
            currentEnemyHealth = value;
            UpdateHealthSprite();
        }
    }

    private void UpdateHealthSprite()
    {
        if (currentEnemyHealth > 1)
        {
            fullHealth.gameObject.SetActive(true);
            halfHealth.gameObject.SetActive(false);
            noHealth.gameObject.SetActive(false);
        }
        else if (currentEnemyHealth == 1)
        {
            fullHealth.gameObject.SetActive(false);
            halfHealth.gameObject.SetActive(true);
            noHealth.gameObject.SetActive(false);
        }
        else if (currentEnemyHealth == 0)
        {
            fullHealth.gameObject.SetActive(false);
            halfHealth.gameObject.SetActive(false);
            noHealth.gameObject.SetActive(true);
        }
    }
}
