using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class WaveSpawn : MonoBehaviour
{
    // spawn set waves after enemy count is reached 

    [SerializeField]
    private int enemiesDestroyed;
    [SerializeField]
    private int enemyWave;

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

    private void Start()
    {
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
            switch (enemyWave)
            {
                case 1:
                    if (enemyWave <= 1 || playerController.enemy < 1)
                    {
                        enemyWave = 1;
                        if (enemiesDestroyed < 1 || playerController.enemy < 1)
                        {
                            enemyWave = 1;
                        }
                        break;
                    }
                    break;
                case 2:
                    if (playerController.enemy >= 10 || enemiesDestroyed >= 10)
                    {
                        enemyWave = 2;
                        if(enemiesDestroyed == 10)
                        {
                            StopCoroutine(Wave1());
                            CancelInvoke("Wave1"); 
                        }
                        break;
                    }
                    break;
                case 3:
                    if (playerController.enemy >= 20 || enemiesDestroyed >= 20)
                    {
                        enemyWave = 3;
                        if (enemiesDestroyed >= 10)
                        {
                            StopCoroutine(Wave2());
                            CancelInvoke("Wave2");
                        }
                        else if(enemiesDestroyed > 20)
                        {
                            StopCoroutine(Wave3());
                            CancelInvoke("Wave3"); 
                        }
                        break;
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
        enemiesDestroyed += enemies;
        if(enemiesDestroyed < 1)
        {
            enemyWave = 1;
            if (enemyWave == 1)
            {
                StartCoroutine(Wave1());
            }
            else if (enemyWave == 2)
            {
                StopCoroutine(Wave1());
            }
        }
        else if(enemiesDestroyed <= 10 || enemies > 10)
        {
            enemyWave = 2;
            if (enemyWave == 2)
            {
                StartCoroutine(Wave2());
            }
            else if (enemyWave == 3)
            {
                StopCoroutine(Wave2()); 
            }
        }
        else if(enemiesDestroyed <= 20 || enemies > 20)
        {
            enemyWave = 3;      
            if (enemyWave == 3)
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
        enemyWave = 1;
        InvokeRepeating("Wave1", 5, 1);
        yield return new WaitForSeconds(3);
        enemy[0].transform.position = container.transform.position;
        Instantiate(enemy[0], container.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(3, 7));
        Instantiate(enemy[0], container.transform.position, Quaternion.identity);
    }

    public IEnumerator Wave2()
    {
        enemyWave = 2;
        InvokeRepeating("Wave2", 10, 1);
        yield return new WaitForSeconds(6);
        enemy[0].transform.position = enemyTwoContainer.transform.position;
        Instantiate(enemy[0], enemyTwoContainer.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(3, 7));
        enemy[2].transform.position = enemyTwoContainer.transform.position;
        Instantiate(enemy[2], enemyTwoContainer.transform.position, Quaternion.identity);
    }

    public IEnumerator Wave3()
    {
        enemyWave = 3;
        InvokeRepeating("Wave3", 20, 1);
        yield return new WaitForSeconds(12);
        enemy[0].transform.position = enemyThreeContainer.transform.position;
        Instantiate(enemy[1], enemyThreeContainer.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(3, 7));
        enemy[2].transform.position = enemyThreeContainer.transform.position;
        Instantiate(enemy[2], enemyThreeContainer.transform.position, Quaternion.identity);
    }
}