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
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Text ammoCount; 

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

    public void AmmoStorage(int ammoAmount)
    {
        ammoAmount--;
        ammoCount.text = "Ammo " + ammoAmount.ToString();
        if (ammoAmount <= 0)
        {
            ammoCount.text = "Ammo: " + ammoAmount.ToString() + " " + "Out of Ammo ";
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

        if (liveSprites.Length <= 0 && livesImage == null)
        {
            return;
        }
        else if(liveSprites.Length > 0 && livesImage != null)
        {
            livesImage.sprite = liveSprites[currentLives];
            if(liveSprites.Length < 0)
            {
                player.GetComponent<BoxCollider2D>().enabled = false; 
                return;
            }
        }

        if(currentLives <= 0 || currentLives <= 2 || currentLives < 2)
        {
            GameOverText.gameObject.SetActive(true);
            restartText.gameObject.SetActive(true);
            gameManager.GameOver(); 
            StartCoroutine(GameOverFlickerRoutine());
            Destroy(livesImage); 
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
