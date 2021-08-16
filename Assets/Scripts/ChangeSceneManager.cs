using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using UnityEngine;
using UnityEngine.UI;

public class ChangeSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject achievements;

    private void Awake()
    {
        DontDestroyOnLoad(this);
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
        Debug.Log("Returned to Main Menu");
        if (arg1.isLoaded)
        {
            GameObject achievements = GetComponent<GameObject>(); 
            if(achievements != null)
            {
                GameObject.Find("Achievements");
                achievements.name = "Achievements";
                Debug.Log("Achievements"); 
            }
            return;
        }
    }

    private void Update()
    {
        if (achievements == null)
        {
            achievements = GameObject.Find("Achievements");
            Debug.Log("Achievements");
            return; 
        }
        else
        {
            if (achievements != null)
            {
                Debug.Log("Achievements Spawned");
                achievements.GetComponent<Button>().interactable = true; 
            }
        }
    }
}
