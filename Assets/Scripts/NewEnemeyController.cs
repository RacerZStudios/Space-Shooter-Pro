using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemeyController : MonoBehaviour
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

    private void Start()
    {
        if (uiManager != null)
        {
            uiManager.enemyText.text = spawnManager.enemyCountDestroyed.ToString();
        }
        else
        {
            uiManager = FindObjectOfType<UI_Manager>();
        }

        if (spawnManager != null)
        {
            return;
        }
        else if (spawnManager == null)
        {
            spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        }

        if (playerController != null)
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
            StartCoroutine(PlayEnemyDeadAnim());
            SpawnManager sM = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            if (sM != null)
            {
                sM.enemyCountDestroyed += 1;
                if (sM.enemyCountDestroyed > 25)
                {
                    // Debug.Assert(sM.enemyCountDestroyed == 3, true);
                    StartCoroutine(sM.BossEnemy());
                }
            }
            if (isDestroyed == true || playerController != null )
            {
                if(sM != null)
                uiManager.enemyText.text = sM.enemyCountDestroyed.ToString();
                int score = 10;
                score += score; 
                UI_Manager uiM = GameObject.Find("Canvas").GetComponent<UI_Manager>();
                uiM.UpdateScore(score);
            }

            if(playerController != null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
                playerController.AddScore(10); 
                return; 
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
    }

    IEnumerator PlayEnemyDeadAnim()
    {
        audioSource.Play();
        // setup New Enemy Aniation
        anim.SetTrigger("OnEnemyDeath");
        yield return new WaitForSeconds(anim.speed); 
        Destroy(gameObject, 2.0f);
        Destroy(GetComponent<Collider2D>()); 
        yield return null; 
    }

    IEnumerator PlayEnemyDeadAnimEMP()
    {
        yield return new WaitForSeconds(3.0f);
        audioSource.Play();
        anim.SetTrigger("OnEnemyDeath");
        yield return new WaitForSeconds(anim.speed);
        Destroy(gameObject, 2.0f);
        Destroy(GetComponent<Collider2D>());
        yield return null;
    }

    IEnumerator NewEnemyAnim()
    {
        if (this.gameObject.tag == "NewEnemy" || this.gameObject != null)
        {
            isNewEnemy = true; 
            if(isNewEnemy == true && anim != null)
            {
                anim.SetBool("IsNewEnemy", true); 
                anim.Play("NewEnemy_Anim");
                if (rB != null)
                {
                    rB.AddForce(Vector3.down * moveSpeed * Time.deltaTime);
                }
            }
        }
        isNewEnemy = false;
        yield return null;
    }

    private void Update()
    {
        while (this != null)
        {
            StartCoroutine(NewEnemyAnim());
            break;
        }

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
            if (transform.position.y < -3)
            {
                Destroy(gameObject);
            }
        }
    }
}
