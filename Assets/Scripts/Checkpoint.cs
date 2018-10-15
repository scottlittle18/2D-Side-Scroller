using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Checkpoint : MonoBehaviour {
    [SerializeField]
    public bool isActivated = false;
    public Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        
    }

    private void Update()
    {
        UpdateAnimation();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //creates a variable that refers to the PlayerController
            PlayerController player = collision.GetComponent<PlayerController>();
            

            //Sends whatever object this script is attached to  to the PlayerController
            player.SetCurrentCheckpoint(this);
            
            
            
        }
    }

    public void SetAsActivated(bool value)
    {
        //anim.SetBool("isActivated", value);
        
        isActivated = value;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (isActivated)
            anim.SetBool("isActivated", true);
        else if (!isActivated)
            anim.SetBool("isActivated", false);
    }
}
