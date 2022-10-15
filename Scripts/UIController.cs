/*
    DISPLAYING THE DISTANCE TEXT
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    Player player;
    public Text distanceText;
    public Text highScoreText;
    public float highScore;
    private float score;

    GameObject results;
    public Text finalDistanceText;

    //Appear the first frame starts
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText =  GameObject.Find("DistanceText").GetComponent<Text>();

        results = GameObject.Find("Results");
        //finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<Text>();
        results.SetActive(false);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetFloat("Highscore");
    }

    // Update is called once per frame
    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";

        if (player.isDead)
        {
            results.SetActive(true);
            finalDistanceText.text = distance + " m";
        }
        highScoreText.text = highScore.ToString();
        if(distance > highScore)
        {
            PlayerPrefs.SetFloat("Highscore", Mathf.FloorToInt(distance));
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");

    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
        
    }
}
