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

    private void Start()
    {
        if(spawnManager == null)
        {
            return; 
        }
        else if(spawnManager != null)
        {
            spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); 
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
            SpawnManager sM = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            if(sM != null)
            {
                sM.enemyCountDestroyed += 1;
                if(sM.enemyCountDestroyed > 0)
                {
                    StartCoroutine(sM.BossEnemy());
                }
            }
            if(isDestroyed == true || playerController != null)
            {
                int score = 10;
                score += score; 
                UI_Manager uiM = GameObject.Find("Canvas").GetComponent<UI_Manager>();
                uiM.UpdateScore(score);
                StartCoroutine(PlayEnemyDeadAnim());
            }

            if (playerController != null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
                playerController.AddScore(10); 
                return; 
            }
            else if(playerController == null && isDestroyed)
            {
                Destroy(gameObject, 2.0f); 
            }
        }

        if(collision.gameObject.tag == "EnemyProjectile")
        {
            audioSource.Play();
            isDestroyed = true;
            if (isDestroyed == true || playerController != null)
            {
                int score = 50;
                score += score;
                UI_Manager uiM = GameObject.Find("Canvas").GetComponent<UI_Manager>();
                uiM.UpdateScore(score);
            }

            if (playerController != null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
                playerController.AddScore(50);
                return;
            }
            else if (playerController == null && isDestroyed)
            {
                Destroy(this);
            }

            gameObject.transform.position += Vector3.left * moveSpeed * Time.deltaTime; 
            StartCoroutine(PlayEnemyDeadAnimEMP());
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
                playerController.GetComponent<PlayerController>().TakeDamage();
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
        yield return new WaitForSeconds(2.0f);
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