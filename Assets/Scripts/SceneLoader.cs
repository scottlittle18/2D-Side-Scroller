using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int currentScene;
    private LifeCounter lifeCounter;
    private ScoreCounter scoreCounter;

    // Use this for initialization
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        lifeCounter = FindObjectOfType<LifeCounter>();
        scoreCounter = FindObjectOfType<ScoreCounter>();
    }

    public void PressedStartButton()
    {
        //Sets the Player's score to 0 when 'Start' is selected on the Main Menu
        PlayerPrefs.SetInt("ScoreCounter", 0);
        SceneManager.LoadScene(currentScene + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ExitCredits()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LevelToRetry"));
    }

    public void Quit()
    {
        //TODO: Debug.Log("Quit Program Input Accepted")
        Debug.Log("Quit Program Input Accepted");
        Application.Quit();
    }
}
