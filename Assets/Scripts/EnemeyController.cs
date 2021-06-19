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

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            playerController.AddScore(10); // pass 10 points 

            isDestroyed = true;
            Destroy(gameObject);
            return; 
        }

        if (collision.gameObject.tag == "Player")
        {
            if (playerController != null)
            {
                playerController.TakeDamage();
            }
        }
    }

    private void Update()
    {
        if(transform.position.y < -3)
        {
            float randomX = Random.Range(-3, 3); 
            transform.position = new Vector3(randomX, spawnPos[0].position.y, 0);
            transform.position = new Vector3(randomX, spawnPos[1].position.y, 0);
            transform.position = new Vector3(randomX, spawnPos[2].position.y, 0);
        }
        else if(!isDestroyed && transform.position.y > -3)
        {
            transform.position += -Vector3.up * moveSpeed * Time.deltaTime;
        }
    }
}
