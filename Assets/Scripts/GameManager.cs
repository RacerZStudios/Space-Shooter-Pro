using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public bool isGameOver;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && isGameOver == true)
        {
            SceneManager.LoadScene(1); // Restart the Game 
        }
        else if (Input.GetKeyDown(KeyCode.R) && isGameOver == false)
        {
            SceneManager.LoadScene(1); // Restart the Game Anytime 
        }

        if(Input.GetKeyDown(KeyCode.Escape)) // Load the Main Menu 
        {
            SceneManager.LoadScene(0); 
        }
    }

    public void GameOver()
    {
        isGameOver = true; 
    }
}