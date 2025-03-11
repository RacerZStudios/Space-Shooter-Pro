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
    public Boss_Health health;
    [SerializeField]
    public bool bossDefeated; 

    // Singleton class? 
    public static BossEnemy_Controller bc;
    [SerializeField]
    private Achievement_Manager achievementManager; 

    private void Awake()
    {
        bc = this; 
    }

    private void Start()
    {
        if (!isDestroyed)
        {
            achievementManager = FindObjectOfType<Achievement_Manager>();
        }

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            isDestroyed = true;
            if (isDestroyed == true && bc.health.currenthealth < 10|| playerController != null)
            {
                int score = 100;
                score ++;
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
        }


        if (playerController != null)
        {
            playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
            playerController.AddScore(50);
        }

        if (collision.gameObject.name == "PlayerController" || collision.gameObject.tag == "Player")
        {
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
        anim.SetTrigger("OnBossDeath");
        yield return new WaitForSeconds(anim.speed);
    }

    IEnumerator BossEnemyAnim()
    {
        if (gameObject.tag == "BossEnemy" || gameObject != null)
        {
            isBossEnemy = true;
            if (achievementManager != null)
            {
                if (!isBossEnemy)
                bossDefeated = true; 
                Achievement_Manager achievement_Manager = GameObject.Find("Achievements_Maanger").GetComponent<Achievement_Manager>();
                if (achievement_Manager.isDestroyed == true)
                {
                    achievement_Manager.isBossEnemy = true;
                }
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

            if(bossDefeated == true)
            {
                StartCoroutine(PlayBossDeadAnim());
                achievementManager.isBossEnemy = true;
                yield return null; 
            }    
        }
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