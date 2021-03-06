using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private float powerUpSpeed = 5;
    [SerializeField]
    private int powerUpID;
    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private PowerUp []powerUp;
    private void Start()
    {
        if (playerController != null)
        {
            playerController = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        }

        if(powerUp == null)
        {
            powerUp[0] = FindObjectOfType<PowerUp>().GetComponent<PowerUp>();
            Debug.Log("Power Up is Null"); 
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerController = collision.transform.GetComponent<PlayerController>();
            AudioSource.PlayClipAtPoint(clip, transform.position); 

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
                    case 3:
                        playerController.AddAmmo(5);
                        break;
                    case 4:
                        playerController.AddHealth(1);
                        break;
                    case 5:
                        playerController.EMPPowerUpActive();
                        break;
                    case 6:
                        playerController.NegativeEffect(0);
                        break;
                    case 7:
                        playerController.SpecialProjectileActive();
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (powerUp.Length != 0 && powerUp != null && player != null)
            {
                powerUp[0].transform.position = Vector3.MoveTowards(powerUp[0].transform.position, player.transform.position, 10);
                if (player == null)
                {                  
                    return; 
                }
            }
        }
    }
}
