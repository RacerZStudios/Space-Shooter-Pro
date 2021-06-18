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

    public bool stopSpawn; 

    private void Start()
    {
        StartCoroutine(SpawnRoutine()); 
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
            yield return new WaitForSeconds(3.0f);
            break; 
        }
        // Instantiate Enemy Prefab 
    }

    private void Update()
    {
        if (stopSpawn == true)
        {
            StopCoroutine(SpawnRoutine());
            return; 
        }
    }

    public void PlayerDead()
    {
        stopSpawn = true; 
    }
}