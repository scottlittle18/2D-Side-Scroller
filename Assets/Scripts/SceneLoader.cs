using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int currentScene;
    private LifeCounter LifeCountKeeper;
    // Use this for initialization
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        LifeCountKeeper = FindObjectOfType<GameManager>();
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
        SceneManager.LoadScene("Main Menu");
    }

    public void Retry()
    {
        LifeCountKeeper.GameOver = false;
        LifeCountKeeper.ResetLifeCount();
        SceneManager.LoadScene(LifeCountKeeper.LevelToRetry);
        //gameManager = FindObjectOfType<GameManager>();
        //gameManager.ResetLifeCount();
    }

    public void Quit()
    {
        //TODO: Debug.Log("Quit Program Input Accepted")
        Debug.Log("Quit Program Input Accepted");
        Application.Quit();
    }
}
