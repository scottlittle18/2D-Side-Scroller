using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Checkpoint : MonoBehaviour
{    
    private bool isActivated;   
    private Animator anim;
    private PlayerController player;

    private void Start()
    {
        IsActivated = false;
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        //UpdateAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {            
            player = collision.GetComponent<PlayerController>();
            player.CurrentCheckpoint = this;
        }
    }

    /// <summary>
    /// Used to Activate or Deactivate checkpoints
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
            UpdateAnimation();
        }
    }

    private void UpdateAnimation()
    {
        if (player.CurrentCheckpoint != null)
        {
            if (isActivated)
                anim.SetBool("isActivated", true);
            else if (!isActivated)
                anim.SetBool("isActivated", false);
        }
    }
}
