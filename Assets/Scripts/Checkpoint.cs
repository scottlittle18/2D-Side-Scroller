using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Checkpoint : MonoBehaviour {
    [SerializeField]
    public bool isActivated = false;
   
    public Animator anim;

    //creates a variable that refers to the PlayerController
    PlayerController player;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        
       
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("The Checkpoint detected the collision");
            player = collision.GetComponent<PlayerController>();
            //SetAsActivated(true);

            player.SetCurrentCheckpoint(this);            
        }
    }

    public void SetAsActivated(bool value)
    {
        //anim.SetBool("isActivated", value);
        Debug.Log("Tried to set as active");
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
