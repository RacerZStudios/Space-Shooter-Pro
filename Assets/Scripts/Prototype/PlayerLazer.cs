using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLazer : MonoBehaviour
{
    private Rigidbody2D rB;
    [SerializeField]
    private float projSpeed;
    
    private void Start()
    {
        rB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rB != null)
        {
            transform.position += Vector3.up * projSpeed;
            Destroy(gameObject, 2); 
        }
    }

    // create collision method 
    private void OnCollisionEnter2D(Collision2D other)
    {
        // get other collision 
        if(other.gameObject.CompareTag("Enemy")) // check for Enemy tag 
        {
            // Destroy other gameobject we hit 
            Destroy(other.gameObject); 
            Destroy(gameObject); // Destroy the projectile for garbage collection cleanup 
        }
    }
}
