using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        if (playerController != null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        if (playerController == null)
        {
            return;
        }


        audioSource = GetComponentInParent<AudioSource>();
        if (audioSource == null)
        {
            return; 
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerController" || collision.gameObject.tag == "Player")
        {
            if(audioSource != null)
            {
                audioSource = GetComponentInParent<AudioSource>();
                audioSource.Play();
            }
            if(audioSource == null)
            {
                audioSource = GameObject.Find("Boss_Enemy").GetComponentInParent<AudioSource>(); 
            }
            // Debug.Log(playerController + "hit"); 
            if (playerController == null)
            {
                playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
            }

            if (playerController != null)
            {
                playerController.GetComponent<PlayerController>().TakeDamage();
            }
        }
    }
}
