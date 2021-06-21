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

    public bool stopSpawn;

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    public IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3); 
        // While Loop (Infinite Loop)
        while(player != null || stopSpawn == false) 
        {
            yield return new WaitForSeconds(Random.Range(3, 7));
            Vector3 spawnPos = new Vector3(Random.Range(-3, 3), 6, 0); 
            GameObject enemyInstance = Instantiate(enemy, spwanPoints[0].transform.position, Quaternion.identity);
            enemyInstance.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }

    public IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3);
        while (player != null && stopSpawn == false)
        {
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp0 = Instantiate(powerUp[0], powerUpSpawn[0].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp1 = Instantiate(powerUp[1], powerUpSpawn[1].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp2 = Instantiate(powerUp[2], powerUpSpawn[2].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3);
        }
    }

    private void Update()
    {
        if (stopSpawn == true)
        {
            StopCoroutine(SpawnRoutine());
            StopCoroutine(SpawnPowerUpRoutine());
        }

        if(player == null)
        {
            stopSpawn = true; 
            Destroy(gameObject); 
        }

        if(enemy == null || enemeyController == null)
        {
            Debug.LogError(enemy.gameObject + enemeyController.ToString() + "are null"); 
            return; 
        }
    }

    public void PlayerDead()
    {
        stopSpawn = true; 
    }
}