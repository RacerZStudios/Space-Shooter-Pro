using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using UnityEngine;
using System.Security.Cryptography;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public bool isGameOver;
    [SerializeField]
    private bool winGame;
    [SerializeField]
    private BossEnemy_Controller BossEnemy_Controller;

    private void Start()
    {
        if (BossEnemy_Controller != null)
        {
            BossEnemy_Controller = FindObjectOfType<BossEnemy_Controller>();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && isGameOver == true || Input.GetButtonDown("Restart") && isGameOver == true)
        {
            // sceme 2 to load into main game 
            SceneManager.LoadScene(1); // Restart the Game 
        }
        else if (Input.GetKeyDown(KeyCode.R) && isGameOver == false || Input.GetButtonDown("Restart") && isGameOver == false)
        {
            SceneManager.LoadScene(1); // Restart the Game Anytime 
        }

        if(Input.GetKeyDown(KeyCode.Escape)) // Load the Main Menu 
        {
            // scene 1 to load main menu 
            SceneManager.LoadScene(0); 
        }

        if (Input.GetKeyDown(KeyCode.R) && winGame == true || Input.GetButtonDown("Restart") && winGame == true) 
        {
            SceneManager.LoadScene(0); // Return to Menu 
        }
    }

    public void EndGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void WinGame()
    {
        winGame = true;
    }

    public void GameOver()
    {
        isGameOver = true; 
    }
}