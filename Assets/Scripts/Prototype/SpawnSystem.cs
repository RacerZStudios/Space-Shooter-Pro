using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    // Create a spawn system of IEnumerator type // CoRoutines 

    //We need to get reference to the enemy prefab to spawn 
    [SerializeField]
    private GameObject enemyPrefab;
    // create refrence to enemy container
    [SerializeField]
    private GameObject enemyContainer;

    // create a stop spawning method 
    private bool stopSpawning = false; 

    private void Start()
    {
        StartCoroutine(SpawnWave()); 
    }
    IEnumerator SpawnWave()
    {
        // CoRoutines use the While loop to loop through data indefinetely 
        while(stopSpawning == false || stopSpawning != true)
        {
            yield return new WaitForSeconds(2); 
            // create the enemy position spawn 
            Vector3 ePos = new Vector3(Random.Range(-8, 8), 8, 0);
            // instantiate / spawn the enemy prefab 
            // create a direct reference to the game object container 
            GameObject enemyInstance = Instantiate(enemyPrefab);
            // set the parent of the enemey game object to the container 
            enemyInstance.transform.parent = enemyContainer.transform; 
            yield return null; 
        }
    }

    public void IsPlayerDead()
    {
        // return stop spawning = true 
        stopSpawning = true;
    }
}
