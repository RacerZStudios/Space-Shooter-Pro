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
        if(bossDefeated && bc.bossDefeated.Equals(true))
        {
            bc.bossDefeated = false; // boss defeated = false on start of game 
        }

        if(bc == null)
        {
            bc = null;
        }
        else
        {
            bc = FindObjectOfType<BossEnemy_Controller>().GetComponent<BossEnemy_Controller>();
        }

        if (boss == null)
        {
            return; 
        }
        else
        {
            if(boss != null)
            {
                boss = FindObjectOfType<GameObject>().GetComponent<GameObject>();
            }
        }   
    }

    private void Update()
    {
        while (bc != null)
        {
            if (bc.enabled.Equals(true))
            {
                if (bc.gameObject == null || bc.isDestroyed == true && bc.health.currenthealth <= 10)
                {
                    bc.bossDefeated = true;
                    if (bc.bossDefeated == true)
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
            else if(isBossDefeated == true)
            {
                bc.bossDefeated = false;
                isBossDefeated= false;
            }
            break;
        }

        if (achievementPanel.activeInHierarchy)
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