using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] 
    public float projSpeed = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
           // SendMessage("UpdateScore", SendMessageOptions.DontRequireReceiver); 
            return; 
        }
    }
    private void Update()
    {
        transform.Translate(Vector3.up * projSpeed * Time.deltaTime); 

        if(transform.position.y > 10)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject); 
        }
    }
}
