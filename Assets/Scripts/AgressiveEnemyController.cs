using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressiveEnemyController : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 0.55f;
    public bool isDestroyed;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Rigidbody2D rB;
    [SerializeField]
    private bool isAgressiveEnemy;
    [SerializeField]
    private float range = 0.3f;
    [SerializeField]
    private SpawnManager spawnManager;
    [SerializeField]
    private UI_Manager uiManager;

    private void Start()
    {
        if (uiManager != null)
        {
            uiManager.enemyText.text = "Enemies Defeated: ";
        }
        else
        {
            uiManager = FindObjectOfType<UI_Manager>().GetComponent<UI_Manager>();
        }

        if (spawnManager == null)
        {
            return;
        }
        else if (spawnManager != null)
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
            if (isDestroyed == true)
            {
                PlayerController playerController = FindObjectOfType<PlayerController>();
                if (playerController != null)
                {
                    playerController.AddScore(15);
                    playerController.AddEnemiesDefeated(1);
                }
            }
            StartCoroutine(PlayEnemyDeadAnim());
            SpawnManager sM = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            if (sM != null)
            {
                sM.enemyCountDestroyed += 1;
                if (sM.enemyCountDestroyed >= 5)
                {
                    StartCoroutine(sM.BossEnemy());
                }
            }
            else if (playerController != null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
                return;
            }
        }

        if (collision.gameObject.name == "PlayerController" || collision.gameObject.tag == "Player")
        {
            audioSource.Play();
            isAgressiveEnemy = true;
            if (playerController == null)
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
        // Create Animation for Agressive Enemy
        anim.SetTrigger("OnEnemyDeath");
        yield return new WaitForSeconds(anim.speed);
        Destroy(gameObject, 2.0f);
        Destroy(GetComponent<Collider2D>());
        yield return null;
    }

    private void Update()
    {
        StartCoroutine(GoToPlayer());

        if (!isDestroyed && transform.position.y > -3)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
        else if (transform.position.y < -3)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator GoToPlayer()
    {
        if(playerController != null)
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            if (player == null)
            {
                Debug.Log("Player is Null");
                yield return null;
            }
            else if (playerController != null)
            {
                yield return null;
            }

            if (player != null)
            {
                if (Vector3.Distance(transform.position, player.transform.position) >= range)
                {
                    yield return new WaitForSeconds(1.5f);
                    if(playerController != null && isAgressiveEnemy.Equals(true))
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, range);
                        yield return new WaitForSeconds(1.5f);
                        if(player != null)
                        {
                            transform.LookAt(player.transform.position);
                        }
                        yield return null; 
                    }
                }
            }
        }
    }
}