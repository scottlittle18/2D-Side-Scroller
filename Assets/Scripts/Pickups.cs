using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour {

    [SerializeField]
    AudioSource coinSFX;

	// Use this for initialization
	void Start () {
        coinSFX = GetComponent<AudioSource>();        
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            coinSFX.Play();

            //Only turning off certain components to make it appear as if the 
            //coin disappeared but will still allow it to play its pickup sound
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
