using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    [SerializeField]
    private int lives;
    private int lifeCounter;
    //[SerializeField]
    private Text lifeText;
    //[SerializeField]
    //private Canvas gameOverScreen;
    private bool gameOver;
    private int levelToRetry;

    [SerializeField]
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

    public int LifeCounter
    {
        get
        {
            return lifeCounter;
        }
        set
        {
            //TODO: Debug.Log("Life Counter Updated");
            Debug.Log("Life Counter Updated");
            lifeCounter = value;
            SetLifeCounterText();
        }
    }

    private void Awake ()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            if (instance.LifeCounter < 1)
                ResetLifeCount();
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        lifeText = GameObject.FindGameObjectWithTag("LifeCounter").GetComponent<Text>();
        ResetLifeCount();
    }

    private void Start()
    {
    }

    public void ResetLifeCount()
    {
        instance.LifeCounter = lives;
    }

    private void CheckForGameOver()
    {
        if (GameOver == true)
        {
            LevelToRetry = SceneManager.GetActiveScene().buildIndex;
            //TODO: Debug.Log("Scene to be retried is " + LevelToRetry);
            Debug.Log("Scene to be retried is " + LevelToRetry);
            SceneManager.LoadScene("Game Over");
            //TODO: Debug.Log("Game Should Be Over");
            Debug.Log("Game Should Be Over");
        }
    }

    private void SetLifeCounterText()
    {
        //TODO: Debug.Log("LifeCounterTextSet");
        Debug.Log("LifeCounterTextSet");
        lifeText.text = "x" + instance.LifeCounter.ToString();
        if (instance.LifeCounter < 0)
            GameOver = true;
    }
}
