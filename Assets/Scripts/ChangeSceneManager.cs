using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using UnityEngine;
using UnityEngine.UI;

public class ChangeSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject achievements;

    public static ChangeSceneManager csMinstance; 

    private void Awake()
    {
        if (csMinstance == null)
        {
            csMinstance = this;
            DontDestroyOnLoad(this);
        }
        else if(csMinstance != this)
        {
            Destroy(this); 
        }
    }

    private void Start()
    {
        if(achievements == null)
        {
            return; 
        }
        else
        {
            achievements = GameObject.Find("Achievements");
        }

        SceneMana(2); 
    }

    public void SceneMana(int level)
    {
        if (level == 2)
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (arg1.isLoaded)
        {
            GameObject achievements = GetComponent<GameObject>(); 
            if(achievements != null)
            {
                GameObject.Find("Achievements");
                achievements.name = "Achievements";
            }
            return;
        }
    }

    private void Update()
    {
        if (achievements == null)
        {
            achievements = GameObject.Find("Achievements");
            return; 
        }
        else
        {
            if (achievements != null)
            {
                achievements.GetComponent<Button>().interactable = true; 
            }
        }
    }
}
