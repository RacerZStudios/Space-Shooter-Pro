using System.Collections;
using System.Collections.Generic;
using TMPro; // declare TextmeshPro library 
using UnityEngine;

public class UIManagerClass : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text livesText;
    [SerializeField]
    private TMP_Text gameOverText;
    [SerializeField]
    private TMP_Text restartLevel; 

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = ("Score: " + scoreText.text);
        livesText.text += (int) 3; 
        livesText.text = ("Lives " + livesText.text);
        gameOverText.text = "";
        restartLevel.text = ""; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update Score 
    public void UpdatePlayerScore(int playerScore)
    {
        scoreText.text = ("Score: ") + playerScore.ToString();
    }

    // Update PlayerLives 
    public void UpdatePlayerLives(int lives)
    {
        livesText.text = ("Lives: ") + lives.ToString(); 
    }

    public void RestartGameLevel(bool restartGameLevel)
    {
        if (restartGameLevel.Equals(true))
        {
            restartLevel.text = " 'R' to Restart Level ";
        }
    }

    private IEnumerator GameOver()
    {
        gameOverText.text = "GAME OVER";
        yield return new WaitForSeconds(3);
        gameOverText.text = ""; 
        yield return new WaitForSeconds(3);
        gameOverText.text = "GAME OVER";
        yield return null; 

    }
}
