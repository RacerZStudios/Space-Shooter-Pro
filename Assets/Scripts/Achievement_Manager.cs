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

    // singleton reference 
    public static Achievement_Manager aCM;

    private void Awake()
    {
        if(aCM == null)
        {
            aCM = this;
            DontDestroyOnLoad(achievementCanvas);
        }
        else if(aCM != this)
        {
            Destroy(achievementCanvas);
        }
    }

    private void Start()
    {
     
    }

    private void Update()
    {
        if(bc == true)
        {
            if(bc.gameObject == null || bc.isDestroyed == true && bc.health.currenthealth <= 10) 
            {
                bc.bossDefeated = true; 
                if(bc.bossDefeated == true)
                {
                    isBossDefeated = true;
                    achievementPanel.gameObject.SetActive(true);
                    if (isBossDefeated == true)
                    {
                        // spawn achievement panel and load win game scene
                        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
                    }
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