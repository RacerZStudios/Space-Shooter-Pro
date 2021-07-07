using System.Collections;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField]
    private int health = 1; 
    private void Start()
    {
        health = 1; 
        while(this != null)
        {
            StartCoroutine(RandomizeShield());
            break; 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayerProjectile")
        {
            PlayerProjectile proj = other.GetComponent<PlayerProjectile>();
            proj.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;   
            health--; 
          //  Debug.Log(" Hit enemy shield"); 
            if(health <= 1)
            {
                Destroy(gameObject);
            }
            return; 
        }
    }

    IEnumerator RandomizeShield()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(Random.Range(0, 3));
        gameObject.SetActive(true);
        yield return null; 
    }
}