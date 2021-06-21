using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 3.0f;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private SpawnManager spawnManager;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip; 

    private void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip; 
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile" || collision.gameObject.name == "Barrier")
        {
            audioSource.Play();
            anim.SetTrigger("OnAsteroidDestroy");
            Destroy(gameObject, 2.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerProjectile" || collision.gameObject.name == "Barrier")
        {
            spawnManager.StartSpawning(); 
            audioSource.Play();
            anim.SetTrigger("OnAsteroidDestroy");
            Destroy(gameObject, 2.0f); 
        }
    }
}
