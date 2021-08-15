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
    private GameObject boss;

    public static Achievement_Manager aCM;

    private void Awake()
    {
        aCM = this; 
    }

    private void Update()
    {
        if(bc == true)
        {
            Debug.Log("Boss Active"); 
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

        if (bC != null)
        {
            if (bC == null)
            {
                Debug.Log("Achievement Unlocked");

                achievementPanel.gameObject.SetActive(true);
                isBossDefeated = true;
                if (isBossDefeated == true)
                {
                    toggle.isOn = true;
                }
            }         
        }
     
        else if (bC == null)
        {
           // Debug.LogError(" Boss in Null");
            return;
        }     
    }
}