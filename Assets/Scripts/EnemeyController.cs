using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyController : MonoBehaviour
{
    [SerializeField] 
    public float moveSpeed = 1.5f;
    public bool isDestroyed;
   // public Transform []spawnPos;
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
            if (playerController == null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
            }

            if (playerController != null)
            {
                playerController.GetComponent<PlayerController>().AddScore(10);
            }
            StartCoroutine(PlayEnemyDeadAnim());
        }

        if (collision.gameObject.name == "PlayerController")
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

        if (!isDestroyed && transform.position.y > -3 && gameObject != null)
        {
            transform.position += -Vector3.up * moveSpeed * Time.deltaTime;
            return; 
        }
    }
}
