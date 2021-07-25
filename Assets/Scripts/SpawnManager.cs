using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Spawn game objects every x amount of seconds 
    // Create a Co-Routine of type IEnumerator -- Yield Events
    // While Loop logic 
    [SerializeField]
    public GameObject enemy;
    [SerializeField]
    public EnemeyController enemeyController;
    public GameObject player;
    [SerializeField]
    private GameObject enemyContainer;
    [SerializeField]
    private GameObject []powerUp;
    [SerializeField]
    public Transform []powerUpSpawn;
    [SerializeField]
    public Transform[] spwanPoints;
    [SerializeField]
    public GameObject newEnemy;
    [SerializeField]
    private GameObject newEnemySpawn;
    [SerializeField]
    private GameObject agressiveEnemy;
    [SerializeField]
    private GameObject agressiveEnemySpawn;
    [SerializeField]
    private GameObject bossEnemy;
    [SerializeField]
    private GameObject bossSpawn; 
    [SerializeField]
    private bool stopSpawn;
    [SerializeField]
    private bool bossDefeated;
    [SerializeField]
    private BossEnemy_Controller bC;

    private void Start()
    {
        if(bC != null)
        {
            bC = GameObject.Find("BossEnemy").GetComponent<BossEnemy_Controller>();
            return; 
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(BossEnemy()); 
    }

    public IEnumerator BossEnemy()
    {
        yield return new WaitForSeconds(40);
        if (bC != null)
        {
            bC = GameObject.Find("BossEnemy").GetComponent<BossEnemy_Controller>();
        }
        while (player != null)
        {
            stopSpawn = false;
            StartCoroutine(SpawnPowerUpRoutine());

            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject bossInstance = Instantiate(bossEnemy, bossSpawn.transform.position, Quaternion.identity);
           // bossInstance.transform.parent = bossSpawn.transform;
            bossInstance.transform.position = new Vector3(bossInstance.transform.position.x, bossSpawn.transform.position.y, bossInstance.transform.position.z); 
            break; 
        }
    }

    public IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3); 
        while(player != null) 
        {
            yield return new WaitForSeconds(Random.Range(3, 7));
            Vector3 spawnPos = new Vector3(Random.Range(-3, 3), 6, 0); 
            GameObject enemyInstance = Instantiate(enemy, spwanPoints[0].transform.position, Quaternion.identity);
            enemyInstance.transform.parent = enemyContainer.transform;
            GameObject newEnemyInstance = Instantiate(newEnemy, spwanPoints[3].transform.position, Quaternion.identity);
            newEnemyInstance.transform.parent = newEnemySpawn.transform;
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject agressiveEnemyInstance = Instantiate(agressiveEnemy, spwanPoints[4].transform.position, Quaternion.identity);
            agressiveEnemyInstance.transform.parent = agressiveEnemySpawn.transform;
            yield return new WaitForSeconds(Random.Range(3, 7));
            break; 
        }
    }

    public IEnumerator SpawnPowerUpRoutine()
    {
        while (player != null && stopSpawn == false)
        {
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp0 = Instantiate(powerUp[0], powerUpSpawn[0].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp1 = Instantiate(powerUp[1], powerUpSpawn[1].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp2 = Instantiate(powerUp[2], powerUpSpawn[2].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp3 = Instantiate(powerUp[3], powerUpSpawn[3].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3);
            GameObject powerUp4 = Instantiate(powerUp[4], powerUpSpawn[4].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3);
            GameObject powerUp5 = Instantiate(powerUp[5], powerUpSpawn[4].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3);
            GameObject powerUp6 = Instantiate(powerUp[6], powerUpSpawn[4].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3);
            GameObject powerUp7 = Instantiate(powerUp[7], powerUpSpawn[5].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3);
            break; 
        }
    }

    private void Update()
    {
        if (player == null)
        {
            PlayerDead(); 
            Destroy(gameObject); 
        }
        
        if (Time.time >= 40f)
        {
            stopSpawn = true;
            if (stopSpawn == true)
            {
                StopCoroutine(SpawnRoutine());
            }
        }

        if (Time.time >= 45f)
        {
            stopSpawn = false;
        }

        if (enemy == null || enemeyController == null)
        {
            Debug.LogError(enemy.gameObject + enemeyController.ToString() + "are null"); 
            return; 
        }

        if(bossEnemy == null && bossEnemy.activeInHierarchy == true || bossDefeated == true)
        {
            Debug.Log("You Win!");
            return; 
        }
    }

    public void PlayerDead()
    {
        stopSpawn = true; 
    }
}