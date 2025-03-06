using System.Collections;
using System.Collections.Generic;
// Get Load Scene Library // Scene Management 
using UnityEngine.SceneManagement; 
using UnityEngine;

public class GameLevelManager : MonoBehaviour
{
    [SerializeField]
    private UIManagerClass uiManager;
    // create a game over bool 
    private bool gameOver; 
    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManagerClass>();
        gameOver = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver.Equals(true))
        {
            // load level 
            if(Input.GetKeyDown(KeyCode.R) && gameOver == true)
            {
                // get scene index to load
                SceneManager.LoadScene(0); 
            }
        }
    }

    public void GetGameOver()
    {
        gameOver = true; // call gameOver bool 
        uiManager.RestartGameLevel(restartGameLevel: true); 
    }
}
