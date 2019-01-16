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
        PlayerPrefs.SetInt("StartingNumberOfLives", 5);
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void PressedStartButton()
    {
        ResetScoreCounterAndHealth();
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
        ResetScoreCounterAndHealth();
        SceneManager.LoadScene(PlayerPrefs.GetInt("LevelToRetry"));
    }

    public void Quit()
    {
        Application.Quit();
    }
    /// <summary>
    /// Call this method to Fully Heal the Player and Reset the ScoreCounter
    /// </summary>
    private void ResetScoreCounterAndHealth()
    {
        PlayerPrefs.SetInt("LifeCounter", PlayerPrefs.GetInt("StartingNumberOfLives"));
        PlayerPrefs.SetInt("ScoreCounter", 0);
    }
}
