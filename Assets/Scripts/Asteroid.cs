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
    [SerializeField]
    private WaveSpawn waveSpawn;
    [SerializeField]
    private GameObject waveSpawnObj; 
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;

    private void Start()
    {
        if (waveSpawnObj != null)
        {
            waveSpawn = FindObjectOfType<WaveSpawn>();
        }
        else
        {
            waveSpawnObj = FindObjectOfType<GameObject>();
            waveSpawn = null;
            return; 
        }

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip; 
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        if (this.gameObject.transform.position.y < -8)
        {
            Destroy(this.gameObject);
        }

        if (waveSpawnObj == null)
        {
            waveSpawnObj = FindObjectOfType<GameObject>().GetComponent<GameObject>();
            return;
        }
        if(waveSpawn == null)
        {
            waveSpawn = FindObjectOfType<WaveSpawn>();
            return; 
        }
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
        if(collision.gameObject.tag == "PlayerProjectile" || collision.gameObject.name == "Barrier" || collision.gameObject.CompareTag("Player"))
        {        
            if (waveSpawnObj.gameObject.activeInHierarchy)
            {
                spawnManager.SpawnPowerUps();
                waveSpawn.StartCoroutine(waveSpawn.Wave1());
                audioSource.Play();
                anim.SetTrigger("OnAsteroidDestroy");
                gameObject.GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, 2.0f);
            }
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            anim.SetTrigger("OnAsteroidDestroy");
            gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 2.0f);
        }
    }
}
