using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Checkpoint : MonoBehaviour
{    
    private bool isActivated;   
    private Animator anim;
    private PlayerController player;
    /// <summary>
    /// Property Used to Activate or Deactivate checkpoints
    /// </summary>
    public bool IsActivated
    {
        get
        {
            return isActivated;
        }
        set
        {
            isActivated = value;
        }
    }

    private void Start()
    {
        IsActivated = false;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {        
        UpdateAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {            
            player = collision.GetComponent<PlayerController>();
            player.CurrentCheckpoint = this;
        }
    }

    private void UpdateAnimation()
    {
        anim.SetBool("isActivated", IsActivated);
    }
}
