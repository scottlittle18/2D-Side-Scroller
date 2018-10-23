using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    int currentScene;

	// Use this for initialization
	void Start () {
        currentScene = SceneManager.GetActiveScene().buildIndex;
	}
	
    public void PressStartButton()
    {
        SceneManager.LoadScene(currentScene + 1);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ExitCredits()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
