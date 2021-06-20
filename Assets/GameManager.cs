using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameOver;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && isGameOver == true)
        {
            SceneManager.LoadScene(1); 
        }
    }

    public void GameOver()
    {
        isGameOver = true; 
    }
}
