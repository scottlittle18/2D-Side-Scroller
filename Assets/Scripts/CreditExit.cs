using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditExit : MonoBehaviour
{
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
            SceneManager.LoadScene("Main Menu");
	}
}
