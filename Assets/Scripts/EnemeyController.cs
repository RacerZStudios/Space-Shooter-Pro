using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyController : MonoBehaviour
{
    [SerializeField] 
    public float moveSpeed = 1.5f;
    public bool isDestroyed;
   // public Transform []spawnPos;
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

    private void Start()
    {
        if(playerController != null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        if (playerController == null)
        {
            return;
        }

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            audioSource.Play();
            isDestroyed = true;
            if(isDestroyed == true || playerController != null)
            {
                int score = 10;
                score += score; 
                UI_Manager uiM = GameObject.Find("Canvas").GetComponent<UI_Manager>();
                uiM.UpdateScore(score); 
                Debug.Log(playerController + "Score");
            }

            if(playerController != null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
                playerController.AddScore(10); 
                return; 
            }
            else if(playerController == null && isDestroyed)
            {
                Destroy(this); 
            }

            StartCoroutine(PlayEnemyDeadAnim());
        }

        if (collision.gameObject.name == "PlayerController" || collision.gameObject.tag == "Player") 
        {
            audioSource.Play();
            Debug.Log(playerController + "hit"); 
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
        anim.SetTrigger("OnEnemyDeath");
        yield return new WaitForSeconds(anim.speed); 
        Destroy(gameObject, 2.0f);
        Destroy(GetComponent<Collider2D>()); 
        yield return null; 
    }

    private void Update()
    {
        if(Time.time > canFire)
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

        //if (!isDestroyed && transform.position.y > -3)
        //{
        //    transform.position += -Vector3.up * moveSpeed * Time.deltaTime;
        //    return; 
        //}
    }
}
