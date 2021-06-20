using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu_Manager : MonoBehaviour
{
   public void LoadGame()
    {
        SceneManager.LoadScene(1); // game scene  
    }
}
