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

    private void Start()
    {
        Destroy(gameObject, 3.0f);
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); 
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerProjectile" || collision.gameObject.name == "Barrier")
        {
            anim.SetTrigger("OnAsteroidDestroy");
            Destroy(gameObject, 2.0f); 
        }
    }
}
