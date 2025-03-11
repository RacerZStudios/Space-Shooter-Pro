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

    private void Start()
    {
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
            SpawnManager sM = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            if (sM != null)
            {
                sM.enemyCountDestroyed += 1;
                if (sM.enemyCountDestroyed > 0)
                {
                    StartCoroutine(sM.BossEnemy());
                }
            }
            if (isDestroyed == true || playerController != null)
            {
                int score = 10;
                score += score;
                UI_Manager uiM = GameObject.Find("Canvas").GetComponent<UI_Manager>();
                uiM.UpdateScore(score);
            }

            if (playerController != null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
                playerController.AddScore(10);
                return;
            }
            else if (playerController == null && isDestroyed)
            {
                Destroy(this);
            }

            StartCoroutine(PlayEnemyDeadAnim());
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