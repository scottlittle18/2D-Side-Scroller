using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Checkpoint : MonoBehaviour {
    [SerializeField]
    public bool isActivated = false;
    private Transform checkpointPosition;
    public Animator anim;

    //creates a variable that refers to the PlayerController
    PlayerController player;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        player = GetComponent<PlayerController>();
        checkpointPosition = this.GetComponent<Transform>();
    }

    private void Update()
    {
        UpdateAnimation();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("The Checkpoint detected the collision");

            SetAsActivated(true);

            player.SetCurrentCheckpoint(checkpointPosition);            
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
