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
    private int maxAmmo; 
    private int finalScore;
    [SerializeField]
    public Slider thurstSlider;
    [SerializeField]
    public Text addHealthText; 

    private void Start()
    {
        addHealthText.gameObject.SetActive(false); 
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
        maxAmmo = 15;
        if(ammoAmount >= maxAmmo)
        {
            // if ammo amount is grater than max amount 
            // set ammo amount to max amount 
            ammoAmount = maxAmmo; 
        }
        ammoCount.text = maxAmmo.ToString();
        ammoAmount--;
        ammoCount.text = "Current Ammo " + ammoAmount.ToString() + " Max Ammo " + " : " + maxAmmo;
        if (ammoAmount <= 0)
        {
            ammoCount.text = "Ammo: " + ammoAmount.ToString() + " / " + "Out of Ammo ";
        }
    }

    public void UpdateScore(int playerScore)
    {
        int finalScore;
        scoretext.text = "Score: " + playerScore.ToString();
        finalScore = playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        // display image sprite 
        // give it a new one based on currentLives index

        if (liveSprites.Length <= 0 && livesImage == null)
        {
            return;
        }

        if(liveSprites.Length > 0 && livesImage != null)
        {
            livesImage.sprite = liveSprites[currentLives];
            if(liveSprites.Length < 0)
            {
                player.GetComponent<BoxCollider2D>().enabled = false; 
            }
        }

        if(currentLives <= 0 || currentLives <= 1)
        {
            GameOverText.gameObject.SetActive(true);
            restartText.gameObject.SetActive(true);
            scoretext.text += finalScore; 
            gameManager.GameOver(); 
            StartCoroutine(GameOverFlickerRoutine());
            Destroy(livesImage); 
        }
    }

    public void AddLivves(int livesToAdd)
    {
        if (liveSprites.Length == 1 && livesImage != null)
        {
            livesToAdd += 1;
            liveSprites[livesToAdd] = livesImage.sprite;
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

    public void StartThrust()
    {
        if(thurstSlider.value <= 1)
        {
            thurstSlider.value-= 0.001f;
        }
    }

    public IEnumerator RegenThrust()
    {
        yield return new WaitForSeconds(3); 
        if(thurstSlider.value <= 1)
        {
            thurstSlider.value+= 0.001f; 
        }
    }
}
