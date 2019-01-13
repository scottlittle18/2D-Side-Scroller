using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeCounter : MonoBehaviour {

    [SerializeField]
    private int lives;
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
            return lifeCounter;
        }
        set
        {
            lifeCounter = value;
            //TODO: Debug.Log("Life Counter Updated");
            Debug.Log("Life Counter Updated");

            PlayerPrefs.SetInt("LifeCounter", lifeCounter);
            //TODO: Debug.Log("LifeCounter PlayerPref Updated");
            Debug.Log("LifeCounter PlayerPref Updated");

            SetLifeCounterText();
        }
    }

    public int LevelToRetry
    {
        get
        {
            return levelToRetry;
        }
        set
        {
            levelToRetry = value;
        }
    }
    #endregion

    private void Awake()
    {
        lifeText = GameObject.FindGameObjectWithTag("LifeCounter").GetComponent<Text>();
    }

    private void CheckForGameOver()
    {
        if (GameOver == true)
        {
            levelToRetry = SceneManager.GetActiveScene().buildIndex;
            //TODO: Debug.Log("Scene to be retried is " + LevelToRetry);
            Debug.Log("Scene to be retried is " + levelToRetry);
            SceneManager.LoadScene("Game Over");
            //TODO: Debug.Log("Game Should Be Over");
            Debug.Log("Game Should Be Over");
        }
    }

    private void SetLifeCounterText()
    {
        lifeText.text = "x" + LifeCountKeeper.ToString();
        //TODO: Debug.Log("LifeCounterTextSet");
        Debug.Log("LifeCounterTextSet");
        if (LifeCountKeeper < 0)
            GameOver = true;
    }

    public void ResetLifeCount()
    {
        LifeCountKeeper = lives;
    }
}
