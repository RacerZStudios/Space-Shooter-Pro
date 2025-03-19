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
    private PlayerController playerController;

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

        while (true)
        {
            playerController.enemy = enemiesDestroyed;
            if (enemiesDestroyed < 1)
            {
                StartCoroutine(Wave1());
            }
            else if (enemiesDestroyed > 5)
            {
                StopCoroutine(Wave1());
                StartCoroutine(Wave2());
            }
            else if (enemyWave >= 10)
            {
                StopCoroutine(Wave2());
                StartCoroutine(Wave3());
            }
            break;
        }
    }

    public IEnumerator Wave1()
    {
        enemyWave = 1;
        InvokeRepeating("Wave1", 5, 3);
        yield return new WaitForSeconds(3);
        enemy[0].transform.position = container.transform.position;
        Instantiate(enemy[0], container.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(3, 7));
        Instantiate(enemy[0], container.transform.position, Quaternion.identity);
    }

    public IEnumerator Wave2()
    {
        enemyWave = 2;
        Instantiate(enemy[0]);
        Instantiate(enemy[2]);
        yield return null;
    }

    public IEnumerator Wave3()
    {
        enemyWave = 3;
        Instantiate(enemy[2]);
        Instantiate(enemy[1]);
        yield return null;
    }
}