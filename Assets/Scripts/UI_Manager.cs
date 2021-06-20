using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    // keep track of score within UI manager 
    // integer data 
    [SerializeField]
    private Text scoretext;
    [SerializeField]
    private Sprite[] liveSprites;
    [SerializeField]
    private Image livesImage;
    [SerializeField] 
    private Text GameOverText;
    [SerializeField]
    private Text restartText;
    [SerializeField]
    private GameManager gameManager; 

    private void Start()
    {
        GameOverText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        if(gameManager == null)
        {
            Debug.LogError("Game Manager is null"); 
        }
    }

    public void UpdateScore(int playerScore)
    {
        scoretext.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        // display image sprite 
        // give it a new one based on currentLives index

        livesImage.sprite = liveSprites[currentLives];

        if(currentLives <= 0)
        {
            GameOverText.gameObject.SetActive(true);
            restartText.gameObject.SetActive(true);
            gameManager.GameOver(); 
            StartCoroutine(GameOverFlickerRoutine()); 
            return; 
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            GameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(3);
            GameOverText.text = "";
            yield return new WaitForSeconds(3);
        }
    }
}
