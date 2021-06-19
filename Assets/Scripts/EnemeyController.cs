using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyController : MonoBehaviour
{
    [SerializeField] 
    public float moveSpeed = 1.5f;
    public bool isDestroyed;
    public Transform []spawnPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            isDestroyed = true;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.transform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
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
