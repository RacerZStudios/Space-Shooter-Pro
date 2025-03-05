using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpClass : MonoBehaviour
{
    [SerializeField]
    private float pUpSpeed;
    // create a PowerUpID 
    [SerializeField]
    private int powerUpID; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move our powerup at a rate of speed 
        transform.Translate(Vector3.down * pUpSpeed * Time.deltaTime);

        if(transform.position.y < -8)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // set power up
            PlayerClass player = collision.GetComponent<PlayerClass>();

            // check if player is not null 
            if (player != null)
            {
                // optimizing with Switch statements 
                switch (powerUpID)
                {
                    case 0:
                        powerUpID = 0;
                        if (powerUpID == 0)
                        {
                            powerUpID = 0;
                            player.TripleShot();
                        }
                        break;
                    case 1:
                        powerUpID = 1;
                        if (powerUpID == 1)
                        {
                            powerUpID = 1;
                            // Speed PowerUp
                            player.SpeedBoost();
                            Debug.Log("Speed Boost Active"); 
                            // Shield, Health, Ammo, etc 
                        }
                        break;
                    case 2: 
                         powerUpID = 2;
                        if(powerUpID == 2)
                        {
                            powerUpID = 2;
                            player.GetShield();
                            Debug.Log("Shield Active");
                        }
                        break; 
                    default:
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}
