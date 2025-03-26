using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro; 
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
    [SerializeField]
    private int maxAmmo = 15; 
    private int finalScore;
    [SerializeField]
    public TMP_Text enemyText;
    [SerializeField]
    private TMP_Text waveText;
    [SerializeField]
    public Slider thurstSlider;
    [SerializeField]
    public Text addHealthText;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    private WaveSpawn waveSpawn;
    [SerializeField]
    bool playerDead = true;

    private void Start()
    {
        playerDead = false;
        if (player)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        addHealthText.gameObject.SetActive(false); 
        GameOverText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);
        gameManager = GameObject.FindObjectOfType<GameManager>(); 
        if(gameManager == null)
        {
            Debug.LogError("Game Manager is null"); 
        }

        if(enemyText != null)
        {
            enemyText.text = "Enemies Defeated: ";
        }
        else
        {
            enemyText.text = FindObjectOfType<TMP_Text>().text; 
            enemyText.text = ""; 
        }

        if(waveText != null)
        {
            waveText.text = "Enemy Wave: 1 "; 
        }
        else
        {
            waveText.text = FindObjectOfType<TMP_Text>().text;
            waveText.text = ""; 
        }

        if(waveSpawn != null)
        {
            return; 
        }
        else
        {
            waveSpawn = FindObjectOfType<WaveSpawn>();
        }
    }

    public void AmmoStorage(int ammoAmount)
    {
        maxAmmo = 15;
        if(ammoAmount >= maxAmmo)
        {
            // if ammo amount is greater than max amount 
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

    public void WaveSpawn(int enemies)
    {
        waveText.text = "Enemy Wave: " + waveSpawn.Wave;
    }

    public void UpdateEnemiesDefeated(int enemyDefeated)
    {
        enemyText.text = ("Enemies Defeated: ") + enemyDefeated.ToString();
    }

    public void UpdateScore(int playerScore)
    {
        scoretext.text = ("Score: ") + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        // display image sprite 
        // give it a new one based on currentLives index
        if(currentLives > 0)
        {
            currentLives--;
            liveSprites.Length.CompareTo(currentLives);
            if (liveSprites.Length > 1 && livesImage != null && player.activeInHierarchy)
            {
                livesImage.sprite = liveSprites[currentLives];
                if (liveSprites.Length < 1 && livesImage != null)
                {
                    player.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            if (currentLives < 1)
            {
                scoretext.text += finalScore;
                gameManager.GameOver();
                Destroy(livesImage);
            }
        }
    }

    public void AddLives(int livesToAdd)
    {
        if (liveSprites.Length == 1 && livesImage != null)
        {
            livesToAdd += 1;
            liveSprites[livesToAdd] = livesImage.sprite;
        }
    }

    public void PlayerDestroyed(bool isPlayer)
    {
        if (isPlayer)
        {
            playerDead = true;
            GameOverText.gameObject.SetActive(true);
            restartText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
            return; 
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(playerDead == true || gameManager.isGameOver == true)
        {
            GameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(3);
            GameOverText.text = "";
            yield return new WaitForSeconds(3);
        }
    }

    public void StartThrust()
    {
       if(playerController.isController == true || Input.GetJoystickNames().Length > 0 || !playerController.isController)
        {
            if (thurstSlider != null)
            {
                thurstSlider.enabled = true;
                if (thurstSlider.value <= 1)
                {
                    thurstSlider.value -= 0.001f;
                }
            }
            else if (thurstSlider == null)
            {
                thurstSlider = GetComponent<Slider>();
                thurstSlider.enabled = false;
                thurstSlider = null;
            }
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
