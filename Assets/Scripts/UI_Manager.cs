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

    public void UpdateScore(int playerScore)
    {
        scoretext.text = "Score: " + playerScore.ToString(); 
    }
}
