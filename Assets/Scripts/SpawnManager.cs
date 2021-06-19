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

    public bool stopSpawn; 

    private void Start()
    {
        StartCoroutine(SpawnRoutine()); 
        StartCoroutine(SpawnPowerUpRoutine());
    }

    public IEnumerator SpawnRoutine()
    {
        // While Loop (Infinite Loop)
        while(player != null || stopSpawn == false) 
        {
            yield return null; // wait 1 frame 
            Vector3 spawnPos = new Vector3(Random.Range(-3, 3), 6, 0); 
            GameObject enemyInstance = Instantiate(enemy, enemeyController.spawnPos[0].position, Quaternion.identity);
            enemyInstance.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(3); 
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
            break;
        }
    }

    private void Update()
    {
        if (stopSpawn == true)
        {
            StopCoroutine(SpawnRoutine());
            StopCoroutine(SpawnPowerUpRoutine());
            return; 
        }

        if(player == null)
        {
            Destroy(gameObject); 
            return; 
        }

        if(enemeyController == null)
        {
            Destroy(gameObject);
            return; 
        }

        if(enemy == null)
        {
            Destroy(gameObject);
            return; 
        }
    }

    public void PlayerDead()
    {
        stopSpawn = true; 
    }
}