using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour 
{    
    [SerializeField]
    [Tooltip("Sprite for when the button is pressed")]
    Sprite buttonPressed;
    [SerializeField]
    [Tooltip("Sprite for when the button is NOT pressed")]
    Sprite buttonUnpressed;
    [Tooltip("Displays the button's current state (pressed/unpressed)")]
    SpriteRenderer menuButtonState;

    private void Awake()
    {
        if ((menuButtonState = null) && (GetComponent<SpriteRenderer>() != null))
        {
            menuButtonState = GetComponent<SpriteRenderer>();
            menuButtonState.sprite = buttonUnpressed;
        }
        else
        {
            //TODO: Debug.Log("Missing SpriteRenderer Component");
            Debug.Log("Missing SpriteRenderer Component");
        }
        
    }
    private void Update()
    {
        InputHandler();
    }

    private void InputHandler()
    {
        if (Input.GetButtonDown("Select"))
        {
            menuButtonState.sprite = buttonPressed;
        }
        if (Input.GetButtonUp("Select"))
        {
            menuButtonState.sprite = buttonUnpressed;
        }
    }	
}
