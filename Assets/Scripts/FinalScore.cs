using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour {

    private Text finalScore;

    private void Awake()
    {
        finalScore = GetComponent<Text>();
        finalScore.text += "\n" + PlayerPrefs.GetInt("ScoreCounter");
    }
}
