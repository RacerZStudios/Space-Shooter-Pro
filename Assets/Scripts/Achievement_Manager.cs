using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using UnityEngine;

public class Achievement_Manager : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private bool isBossDefeated;
    [SerializeField]
    private BossEnemy_Controller bC;
    [SerializeField]
    private GameObject achievementPanel; 

    private void Start()
    {
        DontDestroyOnLoad(this);

        if (toggle != null)
        {
            return; 
        }

        toggle = GameObject.Find("Toggle").GetComponent<Toggle>(); 

        if(bC != null)
        {
            return; 
        }
        else
        {
            bC = GameObject.Find("Boss_Enemy").GetComponent<BossEnemy_Controller>(); 
        }

        if (achievementPanel != null)
        {
            return;
        }
        else
        {
            achievementPanel = GameObject.Find("Achievements_Panel").GetComponent<GameObject>();
        }
    }

    private void Update()
    {
        if(bC != null)
        {
            BossEnemy_Controller BC = GetComponent<BossEnemy_Controller>();
            BC.GetComponent<BossEnemy_Controller>();
            if (BC.isDestroyed.Equals(true) && bC != null || BC.isDestroyed == true)
            {
                Debug.Log("Achievement Unlocked"); 

                achievementPanel.gameObject.SetActive(true); 
                isBossDefeated = true; 
                if(isBossDefeated == true)
                {
                    toggle.isOn = true;
                }    
            }
            else if (bC == null)
            {
                return;
            }
        }
     
    }
}
