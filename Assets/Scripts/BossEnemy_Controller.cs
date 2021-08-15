using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy_Controller : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 1.5f;
    public bool isDestroyed;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Animator anim;
    //[SerializeField]
    //private AudioSource audioSource;
    [SerializeField]
    private GameObject bossProjectile;
    [SerializeField]
    private Transform bProjSpawn;
    private float fireRate = 3;
    private float canFire = -1;
    [SerializeField]
    private Rigidbody2D rB;
    private bool isBossEnemy;
    [SerializeField]
    private Boss_Health health;

    public static BossEnemy_Controller bc;

    private void Awake()
    {
        bc = this; 
    }

    private void Start()
    {
        if (gameObject != null && isBossEnemy == false)
        {
            isBossEnemy = true;
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
        if(rB == null)
        {
            rB.gameObject.AddComponent<Rigidbody2D>(); 
        }

        anim = GetComponent<Animator>();

        if (anim == null)
        {
            return;
        }

        health = GetComponent<Boss_Health>(); 
        if(health == null)
        {
            Debug.Log("Boss Health is Null"); 
        }

      //  audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
           // audioSource.Play();
            isDestroyed = true;
            if (isDestroyed == true || playerController != null)
            {
                int score = 10;
                score += score;
                UI_Manager uiM = GameObject.Find("Canvas").GetComponent<UI_Manager>();
                if(uiM != null)
                {
                    uiM.UpdateScore(score);
                    return; 
                }
            }

            if (playerController != null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
                playerController.AddScore(10);
                return;
            }
            //else if (playerController == null && isDestroyed)
            //{
            //    Destroy(this);
            //}
        }


        if (playerController != null)
        {
            playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
            playerController.AddScore(50);
        }
        //else if (playerController == null && isDestroyed)
        //{
        //    Destroy(this);
        //}

        //    Debug.Log("Emp" + PlayEnemyDeadAnimEMP());
        //    gameObject.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        //    StartCoroutine(PlayEnemyDeadAnimEMP());
        //}

        if (collision.gameObject.name == "PlayerController" || collision.gameObject.tag == "Player")
        {
           // audioSource.Play();
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

    IEnumerator PlayBossDeadAnim()
    {
      //  audioSource.Play();
        anim.SetTrigger("OnBossDeath");
        yield return new WaitForSeconds(anim.speed);
        yield return null;
    }

    IEnumerator BossEnemyAnim()
    {
        if (gameObject.tag == "BossEnemy" || gameObject != null)
        {
            isBossEnemy = true;
            Achievement_Manager achievement_Manager = GameObject.Find("Achievements_Maanger").GetComponent<Achievement_Manager>(); 
            if(achievement_Manager.isDestroyed == true)
            {
                Debug.Log("Achievement Manager is true");
                achievement_Manager.isBossEnemy = true;
                Debug.Log(achievement_Manager.isBossEnemy); 
            }
            if (isBossEnemy == true && anim != null)
            {
                anim.SetBool("IsBossEnemy", true);
                anim.Play("Boss_Enemy");
                if (rB != null)
                {
                    rB.AddForce(Vector3.down * moveSpeed * Time.deltaTime);
                }
            }
        }
      //   isBossEnemy = false;
        yield return null;
    }

    private void LateUpdate()
    {
        while (this != null)
        {
            StartCoroutine(BossEnemyAnim());
            break;
        }

        if (Time.time > canFire && bossProjectile != null)
        {
            fireRate = Random.Range(3, 6);
            canFire = Time.time * fireRate;
            if(this != null)
            {
                GameObject enemyProj = Instantiate(bossProjectile, bProjSpawn.position, Quaternion.identity);
                enemyProj.transform.parent = bProjSpawn.transform;
                return; 
            }
        }
    }
}