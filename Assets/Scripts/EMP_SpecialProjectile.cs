using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_SpecialProjectile : MonoBehaviour
{
    [SerializeField]
    public float projSpeed = 10;
    [SerializeField]
    private bool isEMPProjectile;

    private void Update()
    {
        if (isEMPProjectile == false)
        {
            MoveUp();
        }
        else if (isEMPProjectile == true)
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * projSpeed * Time.deltaTime);

        if (transform.position.y > 5)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * projSpeed * Time.deltaTime);

        if (transform.position.y < -3)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isEMPProjectile == true)
        {
            PlayerController player = collision.transform.GetComponent<PlayerController>();

            if (player != null)
            {
                player.SendMessage("TakeDamage");
                return;
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player hit");
        }
    }
}
