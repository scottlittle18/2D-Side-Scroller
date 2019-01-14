using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour {

    private int scoreCounter;
    private Text scoreText;

	public int ScoreCountKeeper
    {
        get
        {
            return PlayerPrefs.GetInt("ScoreCounter");
        }
        set
        {
            if (value < 0)
                PlayerPrefs.SetInt("ScoreCounter", 0);
            else
                PlayerPrefs.SetInt("ScoreCounter", value);
            SetScoreText();
        }
    }

    private void Awake()
    {
        scoreText = GetComponent<Text>();
        SetScoreText();
    }

    private void SetScoreText()
    {
        scoreText.text = "Score: " + ScoreCountKeeper.ToString();
    }
}
