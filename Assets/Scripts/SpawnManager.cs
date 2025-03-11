using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    private GameObject[] powerUp;
    [SerializeField]
    public Transform[] powerUpSpawn;
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
    [SerializeField]
    public int enemyCountDestroyed;
    [SerializeField]
    private float maxTime = 0;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private UI_Manager uI_Manager;
    [SerializeField]
    private GameObject playerParent;

    private void Start()
    {
        if (uI_Manager == null)
        {
            uI_Manager = FindObjectOfType<UI_Manager>();
        }

        if (player)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        if (bC != null)
        {
            bC = GameObject.Find("BossEnemy").GetComponent<BossEnemy_Controller>();
            return;
        }

        enemyCountDestroyed = 0;
        InvokeRepeating("SpawnEnemy1", 5, 3);
        InvokeRepeating("SpawnEnemy2", 15, 3);
        InvokeRepeating("SpawnEnemy3", 3, 3);
    }

    public IEnumerator SpawnEnemy1()
    {
        while(this != null)
        {
            yield return new WaitForSeconds(3);
            GameObject enemyInstance = Instantiate(enemy, spwanPoints[0].transform.position, Quaternion.identity); // normal enemy 
            enemyInstance.transform.parent = enemyContainer.transform;

            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp0 = Instantiate(powerUp[0], powerUpSpawn[0].transform.position, Quaternion.identity); // triple shot 
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp1 = Instantiate(powerUp[1], powerUpSpawn[1].transform.position, Quaternion.identity); // speed 
        }
    }

    public IEnumerator SpawnEnemy2()
    {
        while (this != null)
        {
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject newEnemyInstance = Instantiate(newEnemy, spwanPoints[3].transform.position, Quaternion.identity); // new enemy 
            newEnemyInstance.transform.parent = newEnemySpawn.transform;

            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp2 = Instantiate(powerUp[2], powerUpSpawn[2].transform.position, Quaternion.identity); // Shield 
        }
    }

    public IEnumerator SpawnEnemy3()
    {
        while (this != null)
        {
            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject agressiveEnemyInstance = Instantiate(agressiveEnemy, spwanPoints[4].transform.position, Quaternion.identity); // argressive enemy 
            agressiveEnemyInstance.transform.parent = agressiveEnemySpawn.transform;

            yield return new WaitForSeconds(Random.Range(3, 7));
            GameObject powerUp3 = Instantiate(powerUp[3], powerUpSpawn[3].transform.position, Quaternion.identity); // Ammo 
        }
    }

    public IEnumerator SpawnHealth()
    {
        while(this != null)
        {
            yield return new WaitForSeconds(30);
            GameObject powerUp4 = Instantiate(powerUp[4], powerUpSpawn[4].transform.position, Quaternion.identity); // Health (Rare)
            yield return new WaitForSeconds(30);
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy1());
        StartCoroutine(SpawnEnemy2());
        StartCoroutine(SpawnEnemy3());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(SpawnHealth()); 
    }

    public IEnumerator BossEnemy()
    {
        while (maxTime >= 0.130f || maxTime <= 0.131f)
        {
            StartCoroutine(WaitTime());
            break;
        }

        yield return new WaitForSeconds(2);
        // yield return new WaitForSeconds(40); waits for 40 sec // spawns after 40 sec 
        if (bC != null)
        {
            bC = GameObject.Find("BossEnemy").GetComponent<BossEnemy_Controller>();
        }

        while (player != null)
        {
            stopSpawn = false;
            if(this != null)
            {
                StartCoroutine(SpawnPowerUpRoutine());
            }

            if(stopSpawn == false)
            {
                yield return new WaitForSeconds(3); 
                GameObject bossInstance = Instantiate(bossEnemy, bossSpawn.transform.position, Quaternion.identity);
                bossInstance.transform.position = new Vector3(bossInstance.transform.position.x, bossSpawn.transform.position.y, bossInstance.transform.position.z);
                yield return new WaitForSeconds(0.5f); // wait to spawn boss 
                Destroy(this);
            }        
            break; 
        }
    }

    public IEnumerator SpawnPowerUpRoutine()
    {
        while (this != null)
        {
            GameObject powerUp5 = Instantiate(powerUp[5], powerUpSpawn[4].transform.position, Quaternion.identity); // EMP 
            yield return new WaitForSeconds(3);
            GameObject powerUp6 = Instantiate(powerUp[6], powerUpSpawn[4].transform.position, Quaternion.identity); // Negative 
            yield return new WaitForSeconds(5);
            GameObject powerUp7 = Instantiate(powerUp[7], powerUpSpawn[5].transform.position, Quaternion.identity); // Special 
            yield return new WaitForSeconds(10);
            GameObject powerUp4 = Instantiate(powerUp[3], powerUpSpawn[3].transform.position, Quaternion.identity); // Ammo  
            yield return new WaitForSeconds(15);
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

        if (maxTime > 0.131f)
        {
            StopCoroutine(WaitTime());
        }
    }

    private IEnumerator WaitTime()
    {
        maxTime += Time.deltaTime;
        yield return new WaitForSeconds(2); 
        while (enemyCountDestroyed >= 3)
        {
            yield return new WaitForSeconds(2);
            StartCoroutine(BossEnemy());
            if(bossSpawn == true)
            {
                if(player != null)
                {
                    if(playerController)
                    {
                        uI_Manager.AmmoStorage(30);
                        playerController.AddFinalAmmo(30);
                    }
                }
                break; 
            }
            if (enemyCountDestroyed >= 3 && maxTime > 0.13f)
            {
                GameObject powerUp4 = Instantiate(powerUp[3], powerUpSpawn[3].transform.position, Quaternion.identity); // Spawn Ammo for Boss Battle  
                yield return new WaitForSeconds(15);
                StopCoroutine(BossEnemy());
            }
            break;
        }

        yield return null; 
    }

    public void PlayerDead()
    {
        stopSpawn = true; 
    }
}