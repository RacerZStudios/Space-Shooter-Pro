using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // get scene manager namespace library 
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void LoadMainGame()
    {
        SceneManager.LoadScene(0); 
    }
}
