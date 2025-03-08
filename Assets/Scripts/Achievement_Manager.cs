using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using UnityEngine;

public class Achievement_Manager : BossEnemy_Controller
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private bool isBossDefeated;
    [SerializeField]
    private BossEnemy_Controller bC;
    [SerializeField]
    private GameObject achievementPanel;
    [SerializeField]
    private GameObject achievementCanvas; 
    [SerializeField]
    private GameObject boss;

    public static Achievement_Manager aCM;

    private void Awake()
    {
        aCM = this; 
    }

    private void Start()
    {
        DontDestroyOnLoad(achievementCanvas);
    }

    private void Update()
    {
        if(bc == true)
        {
            //Debug.Log("Boss Active"); 
            //if(bc.CompareTag("BossEnemy"))
            //{
            //    Debug.Log("Boss"); 
            //}
            if(bc.gameObject == null || bc.isDestroyed == true && bc.health.currenthealth <= 20) 
            {
                bc.bossDefeated = true; 
                if(bc.bossDefeated == true)
                {
                    Debug.Log("Achievement Unlocked"); // this works 

                    achievementPanel.gameObject.SetActive(true);
                    isBossDefeated = true;
                    if (isBossDefeated == true)
                    {
                        // spawn achievement panel 
                        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
                    }
                  //  Debug.LogError("Boss Defeated");
                }
            }
        }

        if(achievementPanel.activeInHierarchy)
        {
            achievementPanel.GetComponent<GameObject>();
        }

        if(boss??false)
        {
            isBossDefeated = false;
            bC.GetComponent<BossEnemy_Controller>();
        }

        if (boss == null && isBossDefeated == true)
        {
            achievementPanel.SetActive(true);
        }   
    }
}