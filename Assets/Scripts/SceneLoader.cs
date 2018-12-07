using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int currentScene;
    // Use this for initialization
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void PressedStartButton()
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
        //TODO: Debug.Log("Quit Program Input Accepted")
        Debug.Log("Quit Program Input Accepted");
        Application.Quit();
    }
}
