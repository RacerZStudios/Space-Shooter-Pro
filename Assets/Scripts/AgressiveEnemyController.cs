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

    private void Start()
    {
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
            if (isDestroyed == true || playerController != null)
            {
                int score = 10;
                score += score;
                UI_Manager uiM = GameObject.Find("Canvas").GetComponent<UI_Manager>();
                uiM.UpdateScore(score);
                // Debug.Log(playerController + "Score");
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
            // Debug.Log(playerController + "hit"); 
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
      //  anim.SetTrigger("OnEnemyDeath");
      //  yield return new WaitForSeconds(anim.speed);
        Destroy(gameObject, 2.0f);
        Destroy(GetComponent<Collider2D>());
        yield return null;
    }

    //IEnumerator AgressiveEnemyAnim()
    //{
    //    if (this.gameObject.tag == "NewEnemy" || this.gameObject != null)
    //    {
    //        isAgressiveEnemy = true;
    //        if (isAgressiveEnemy == true && anim != null)
    //        {
    //            anim.SetBool("IsNewEnemy", true);
    //            anim.Play("NewEnemy_Anim");
    //            if (rB != null)
    //            {
    //                rB.AddForce(Vector3.down * moveSpeed * Time.deltaTime);
    //            }
    //        }
    //    }
    //    isAgressiveEnemy = false;
    //    yield return null;
    //}

    private void Update()
    {
        //while (this != null)
        //{
        //    StartCoroutine(AgressiveEnemyAnim());
        //    break;
        //}

        if (!isDestroyed && transform.position.y > -3)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            if (transform.position.y < -3)
            {
                Destroy(gameObject);
            }
        }

        StartCoroutine(GoToPlayer()); 
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
                    if(playerController != null)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, range);
                        yield return new WaitForSeconds(1.5f);
                        transform.LookAt(player.transform.position);
                    }
                }
            }
        }
    }
}