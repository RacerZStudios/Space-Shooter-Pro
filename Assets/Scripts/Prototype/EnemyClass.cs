using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    // create enemy class that will move down towards the player 
    [SerializeField]
    private float speed = 4.0f;

    private PlayerClass PlayerClass; 

    // Start is called before the first frame update
    void Start()
    {
        PlayerClass = GameObject.FindObjectOfType<PlayerClass>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.position; 
        // move the enemy down 4 units per second 
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // set random range on x axis 
        float randomX = Random.Range(8, -8); 

        // reset enemy position 
        if(direction.y < -8)
        {
            transform.position = new Vector3(randomX, 8, 0); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (PlayerClass != null)
            {
                PlayerClass.TakeDamage();
            }
            return; 
        }

        // create a tag for the player  lazer and add score 
        if(other.gameObject.CompareTag("PlayerLazer"))
        {
            Destroy(other.gameObject);
            if (PlayerClass != null)
            {
                PlayerClass.AddScore(10);
            }
            Destroy(gameObject);
            return; 
        }
    }
}
