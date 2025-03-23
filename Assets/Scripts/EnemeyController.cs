using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyController : MonoBehaviour
{
    [SerializeField] 
    public float moveSpeed = 1.5f;
    public bool isDestroyed;
   [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private GameObject enemyProjectile;
    [SerializeField]
    private Transform eProjSpawn; 
    private float fireRate = 3;
    private float canFire = -1;
    [SerializeField]
    private Rigidbody2D rB;
    [SerializeField]
    private bool isNewEnemy;
    [SerializeField]
    private SpawnManager spawnManager;
    [SerializeField]
    private UI_Manager uiManager;
    [SerializeField]
    private WaveSpawn waveSpawn;

    private void Start()
    {
        if(waveSpawn == null)
        {
            waveSpawn = FindObjectOfType<WaveSpawn>();
        }   
        
        if (uiManager != null)
        {
            uiManager.enemyText.text = "Enemies Defeated: " + spawnManager.enemyCountDestroyed.ToString();
        }
        else
        {
            uiManager = FindObjectOfType<UI_Manager>();
        }

        if(spawnManager == null)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
            return; 
        }
        else if(spawnManager != null)
        {
            if (spawnManager.enemyCountDestroyed > 0)
            {
                spawnManager.enemyCountDestroyed = 0;
            }
        }

        if(playerController != null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        if (playerController == null)
        {
            return;
        }

        rB = GetComponent<Rigidbody2D>(); 

        anim = GetComponent<Animator>();

        if (anim == null)
        {
            return; 
        }

        audioSource = GetComponent<AudioSource>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            audioSource.Play();
            isDestroyed = true;
            if (isDestroyed == true)
            {
                PlayerController playerController = FindObjectOfType<PlayerController>();
                WaveSpawn waveSpawn = FindObjectOfType<WaveSpawn>();
                if(playerController != null)
                {
                    playerController.AddScore(10);
                    playerController.AddEnemiesDefeated(1);
                }
            }
            StartCoroutine(PlayEnemyDeadAnim());
            if (spawnManager)
            {
                if (spawnManager != null)
                {
                    spawnManager.enemyCountDestroyed += 1;
                    if (spawnManager.enemyCountDestroyed >= spawnManager.enemiesDefeatedToBoss) // spawn boss  
                    {
                        StartCoroutine(spawnManager.BossEnemy());
                    }
                    return;
                }
            }
            else if (playerController != null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
                return;
            }
            else if (playerController == null && isDestroyed)
            {
                Destroy(gameObject, 2.0f);
            }
        }

        if (collision.gameObject.name == "PlayerController" || collision.gameObject.tag == "Player") 
        {
            audioSource.Play();
            if(playerController == null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>(); 
            }

            if (playerController != null)
            {
                playerController.GetComponent<PlayerController>().TakeDamage(); // update lives count 
            }
        }
        
        // get barrier collision 
        if(collision.gameObject.CompareTag("Barrier"))
        {
            Destroy(gameObject); 
        }
    }

    IEnumerator PlayEnemyDeadAnim()
    {
        audioSource.Play();
        anim.SetTrigger("OnEnemyDeath");
        yield return new WaitForSeconds(anim.speed);
        Destroy(GetComponent<Collider2D>());
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
        yield return null; 
    }

    IEnumerator PlayEnemyDeadAnimEMP()
    {
        yield return new WaitForSeconds(3.0f);
        audioSource.Play();
        anim.SetTrigger("OnEnemyDeath");
        yield return new WaitForSeconds(anim.speed);
        Destroy(GetComponent<Collider2D>());
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
        yield return null;
    }

    private void Update()
    {
        if (Time.time > canFire)
        {
            fireRate = Random.Range(3, 6);
            canFire = Time.time * fireRate;
            GameObject enemyProj = Instantiate(enemyProjectile, eProjSpawn.position, Quaternion.identity);
            PlayerProjectile[] proj = enemyProj.GetComponentsInChildren<PlayerProjectile>();

            for(int i = 0; i < proj.Length; i++)
            {
                proj[i].AssignEnemyProjectile(); 
            }
        }


        if (!isDestroyed && transform.position.y > -3)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
        else if (transform.position.y < -3)
        {
            Destroy(gameObject);
        }
    }
}