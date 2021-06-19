using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private PlayerController playerController;
    public float powerUpSpeed = 5;
    [SerializeField]
    public int powerUpID; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerController = collision.transform.GetComponent<PlayerController>(); 
            if(playerController != null)
            {
                switch(powerUpID)
                {
                    case 0:
                        playerController.TrippleShotActive();
                        break;
                    case 1:
                        playerController.SpeedBoostActive(); 
                        break; 
                    case 2:
                        playerController.ShieldActive(); 
                        break; 
                    default:
                        Debug.Log("Default value"); 
                        break; 
                }
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