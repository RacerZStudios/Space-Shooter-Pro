using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] 
    public float projSpeed = 10;
    [SerializeField]
    private bool isEnemyProjectile; 

    private void Update()
    {
        if (isEnemyProjectile == false)
        {
            MoveUp();
        }
        else if(isEnemyProjectile == true)
        {
            MoveDown(); 
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * projSpeed * Time.deltaTime);

        if (transform.position.y > 10)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * projSpeed * Time.deltaTime);

        if (transform.position.y < -3)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    public void AssignEnemyProjectile()
    {
        isEnemyProjectile = true; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && isEnemyProjectile == false)
        {
            PlayerController player = collision.transform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddEnemiesDefeated(1);
                player.UpdateWaves(player.enemy);
                return; 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && isEnemyProjectile == true)
        {
            other.SendMessage("TakeDamage"); 
        }
    }
}
