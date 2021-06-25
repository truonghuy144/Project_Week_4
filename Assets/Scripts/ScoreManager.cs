using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public Text scoreText;
    public Text highScoreText;

    private int score = 0;
    private int highscore = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = "SCORE:  " + score.ToString();
        highScoreText.text = "HIGHSCORE:  " + highscore.ToString();
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = "SCORE:  " + score.ToString();
        if (highscore < score)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
