using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyController : MonoBehaviour
{
    [SerializeField] 
    public float moveSpeed = 1.5f;
    public bool isDestroyed;
    public Transform []spawnPos;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private SpawnManager spawnManager;
    [SerializeField]
    private Animator anim; 

    private void Start()
    {
        if(playerController != null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            return; 
        }

        anim = GetComponent<Animator>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        { 
            isDestroyed = true;
            playerController.AddScore(10);
            StartCoroutine(PlayEnemyDeadAnim()); 
            return; 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerController.TakeDamage();
            return; 
        }
    }

    IEnumerator PlayEnemyDeadAnim()
    {
        anim.SetTrigger("OnEnemyDeath");
        yield return new WaitForSeconds(anim.speed); 
        Destroy(gameObject, 2.0f);
        yield return null; 
    }

    private void Update()
    {
        if(playerController.gameObject == null)
        {
            Destroy(gameObject);
            Destroy(playerController);
            Destroy(spawnManager); 
        }

        if (transform.position.y < -3 && gameObject != null)
        {
            float randomX = Random.Range(-3, 3); 
            transform.position = new Vector3(randomX, spawnPos[0].position.y, 0);
            transform.position = new Vector3(randomX, spawnPos[1].position.y, 0);
            transform.position = new Vector3(randomX, spawnPos[2].position.y, 0); 
        }
        else if(!isDestroyed && transform.position.y > -3 && gameObject != null)
        {
            transform.position += -Vector3.up * moveSpeed * Time.deltaTime;
        }

        if(spawnPos[0] == null || spawnPos[1] == null || spawnPos[2] == null)
        {
            Destroy(spawnPos[0]);
            Destroy(spawnPos[1]);
            Destroy(spawnPos[2]);
        }
    }
}
