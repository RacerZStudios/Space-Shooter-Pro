using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditorInternal;
using UnityEngine;

public class WaveSpawn : MonoBehaviour
{
    // spawn set waves after enemy count is reached 

    [SerializeField]
    private int enemiesDestroyed;
    [SerializeField]
    private int EnemyWave { get; set; } // property accessor 

    [SerializeField]
    private GameObject[] enemy;

    [SerializeField]
    private SpawnManager spawnManager;

    [SerializeField]
    private Transform container;

    [SerializeField]
    private Transform enemyTwoContainer;

    [SerializeField]
    private Transform enemyThreeContainer;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private UI_Manager uIManager;

    private static bool waveActive = false; 

    private static WaveSpawn instance;

    public bool WaveIsActive
    {
        get
        {
            return waveActive; 
        }
        set
        {
            waveActive = value;
        }
    }

    public int Wave
    {
        get
        {
            return EnemyWave;
        }
        set
        {
            EnemyWave = value;
        }
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this; 
            instance = FindObjectOfType<WaveSpawn>();
        }

        if (spawnManager == null)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        while (this != null)
        {
            switch (EnemyWave)
            {
                case 1:
                    if (EnemyWave <= 1 || playerController.enemy < 10 || enemiesDestroyed < 10)
                    {
                        EnemyWave = 1;
                    }
                    break;
                case 2:
                    if (playerController.enemy > 10 || enemiesDestroyed > 10)
                    {
                        EnemyWave = 2;
                        if(enemiesDestroyed == 10)
                        {
                            StopCoroutine(Wave1());
                            CancelInvoke("Wave1"); 
                        }
                    }
                    break;
                case 3:
                    if (playerController.enemy >= 20 || enemiesDestroyed >= 20)
                    {
                        EnemyWave = 3;
                        if (enemiesDestroyed > 10)
                        {
                            StopCoroutine(Wave2());
                            CancelInvoke("Wave2");
                        }
                        else if(enemiesDestroyed > 20)
                        {
                            StopCoroutine(Wave3());
                            CancelInvoke("Wave3"); 
                        }
                    }
                    break;
                default:
                    break;
            }

            break; 
        }
    }

    public void EnemiesDestroyed(int enemies)
    {   
        if(enemiesDestroyed < 1)
        {
            enemiesDestroyed += enemies;
            EnemyWave = 1;
            if (EnemyWave == 1)
            {
                StartCoroutine(Wave1());
            }
            else if (enemies >= 10)
            {
                StopCoroutine(Wave1());
            }
        }
        else if(enemiesDestroyed <= 10 || enemies > 10)
        {
            enemiesDestroyed += enemies;
            EnemyWave = 2;
            if (EnemyWave == 2 || enemies > 10)
            {   
                StartCoroutine(Wave2());
            }
            else if (EnemyWave == 3 || enemies == 20)
            {
                StopCoroutine(Wave2()); 
            }
        }
        else if(enemiesDestroyed <= 20 || enemies > 20)
        {
            enemiesDestroyed += enemies;
            EnemyWave = 3;      
            if (EnemyWave == 3 || enemies == 20)
            {
                StartCoroutine(Wave3()); 
            }
            else if (enemiesDestroyed >= 20 || enemies >= 20)
            {
                StopCoroutine(Wave3());
            }
        }
    }

    public IEnumerator Wave1()
    {
        EnemyWave = 1;
        InvokeRepeating("Wave1", 5, 3);
        yield return new WaitForSeconds(6);
        enemy[0].transform.position = container.transform.position;
        Instantiate(enemy[0], container.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(3, 7));
        Instantiate(enemy[0], container.transform.position, Quaternion.identity);
        yield break;
    }

    public IEnumerator Wave2()
    {
        EnemyWave = 2;
        InvokeRepeating("Wave2", 10, 4);
        yield return new WaitForSeconds(12);
        enemy[0].transform.position = enemyTwoContainer.transform.position;
        Instantiate(enemy[0], enemyTwoContainer.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(3, 14));
        enemy[2].transform.position = enemyTwoContainer.transform.position;
        Instantiate(enemy[2], enemyTwoContainer.transform.position, Quaternion.identity);
        yield break;
    }

    public IEnumerator Wave3()
    {
        EnemyWave = 3;
        InvokeRepeating("Wave3", 15, 6);
        yield return new WaitForSeconds(24);
        enemy[0].transform.position = enemyThreeContainer.transform.position;
        Instantiate(enemy[1], enemyThreeContainer.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(6, 28));
        enemy[2].transform.position = enemyThreeContainer.transform.position;
        Instantiate(enemy[2], enemyThreeContainer.transform.position, Quaternion.identity);
        yield break;
    }
}