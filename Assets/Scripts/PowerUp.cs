using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] 
    PlayerController playerController;
    public float powerUpSpeed = 15;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerController = collision.transform.GetComponent<PlayerController>(); 
            if(playerController != null)
            {
                playerController.TrippleShotActive(); 
            }
            Destroy(gameObject); 
            return; 
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.down * powerUpSpeed * Time.deltaTime); 
        if(transform.position.y < -3)
        {
            Destroy(gameObject); 
        }
    }
}
