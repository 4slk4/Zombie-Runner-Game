using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;
    private float score;
    public float highScore;
   
    void Start()
    {
        highScore = PlayerPrefs.GetFloat("Highscore");
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            score += 50 * Time.deltaTime;
            scoreText.text = ((int)score).ToString();
        }
        
        //highScoreText.text = highScore.ToString();
        if (highScoreText != null)
        {
            highScoreText.text = highScore.ToString();
        }
        
        if(score > highScore)
        {
            PlayerPrefs.SetFloat("Highscore", Mathf.FloorToInt(score));
        }
    }
}
