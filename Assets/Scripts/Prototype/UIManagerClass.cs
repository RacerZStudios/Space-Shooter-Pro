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

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = ("Score: " + scoreText.text);
        livesText.text = ("Lives " + livesText.text);
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

    public void UpdateLives(int lives)
    {
        livesText.text = ("Lives: ") + lives.ToString(); 
    }
}
