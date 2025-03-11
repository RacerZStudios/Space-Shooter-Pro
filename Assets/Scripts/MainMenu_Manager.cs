using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu_Manager : MonoBehaviour
{
    public GameObject[] menuButtons;
    public GameObject[] interactButtons; 
    private void Start()
    {
        menuButtons = new GameObject[menuButtons.Length];
        if(menuButtons.Length > 0 )
        {
            menuButtons[0] = gameObject;
            menuButtons[1] = gameObject;
        }

        interactButtons = new GameObject[interactButtons.Length];
        if(interactButtons.Length > 0 )
        {
            interactButtons[0] = gameObject;
        }
    }

    public void NewGame()
    {
        menuButtons = new GameObject[menuButtons.Length];
        if (menuButtons.Length > 0)
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                menuButtons[0] = gameObject;
                float v = Input.GetAxis("Vertical"); 
                if(v > 0)
                {
                    Debug.Assert(v > 0);
                }
            }
        }
    }

    public void Controls()
    {
        menuButtons = new GameObject[menuButtons.Length];
        if (menuButtons.Length > 0)
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                menuButtons[1] = gameObject;
                float h = Input.GetAxis("Horizontal");
                if (h > 0)
                {
                    Debug.Assert(h > 0);
                }

                if(Input.GetButtonDown("Cancel"))
                {
                    interactButtons[0] = gameObject;
                    interactButtons[0].SetActive(false);
                }

                if (Input.GetJoystickNames().ToString().Contains(("Back")))
                {
                    interactButtons[0] = gameObject;
                    interactButtons[0].SetActive(false);
                    Debug.Assert(true); 
                }
            }
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1); // Load Game Scene  
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit Game 
    }
}
