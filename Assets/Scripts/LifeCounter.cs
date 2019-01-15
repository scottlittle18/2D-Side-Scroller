using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeCounter : MonoBehaviour {

    [SerializeField]
    [Tooltip("The Number of Lives That the Player Starts the Game with")]
    private int playerLives;
    private int lifeCounter;
    //[SerializeField]
    private Text lifeText;
    private bool gameOver;
    private int levelToRetry;

    #region Properties
    public bool GameOver
    {
        get
        {
            return gameOver;
        }
        set
        {
            gameOver = value;
            CheckForGameOver();
        }
    }

    public int LifeCountKeeper
    {
        get
        {
            return PlayerPrefs.GetInt("LifeCounter");
        }
        set
        {
            PlayerPrefs.SetInt("LifeCounter", value);
            SetLifeCounterText();
        }
    }

    public int LevelToRetry
    {
        get
        {
            return PlayerPrefs.GetInt("LevelToRetry");
        }
        set
        {
            PlayerPrefs.SetInt("LevelToRetry", value);
        }
    }
    #endregion

    private void Awake()
    {
        lifeText = GetComponent<Text>();
        if (0 < LifeCountKeeper && LifeCountKeeper < 6)
        {
            SetLifeCounterText();
        }            
        else
            ResetLifeCount();
    }

    private void Start()
    {
        GameOver = false;
    }

    private void CheckForGameOver()
    {
        if (GameOver == true)
        {
            LevelToRetry = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("Game Over");
        }
    }

    private void SetLifeCounterText()
    {
        lifeText.text = "x" + LifeCountKeeper.ToString();
        if (LifeCountKeeper < 0)
            GameOver = true;
    }

    public void ResetLifeCount()
    {
        LifeCountKeeper = playerLives;
    }
}
