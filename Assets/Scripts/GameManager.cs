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
            SceneManager.LoadScene(1); 
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); 
        }
    }

    public void GameOver()
    {
        isGameOver = true; 
    }
}
