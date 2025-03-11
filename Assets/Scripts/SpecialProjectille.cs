using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialProjectille : MonoBehaviour
{
    [SerializeField]
    public float projSpeed = 10;
    [SerializeField]
    private bool isEnemyProjectile;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private bool hasSpecialProjectile;
    [SerializeField]
    private float range = 2;

    private void Start()
    {
        if (target != null)
        {
            target = GameObject.FindGameObjectWithTag("BossEnemy").GetComponent<GameObject>();
        }
        else if (target == null)
        {
            return;
        }
    }

    private void Update()
    {
        if (isEnemyProjectile == false)
        {
            MoveUp();
        }
        else if (isEnemyProjectile == true)
        {
            MoveDown();
        }

        StartCoroutine(GoToEnemy()); 
    }

    private IEnumerator GoToEnemy()
    {
        if (target != null)
        {
            BossEnemy_Controller target = GameObject.FindGameObjectWithTag("BossEnemy").GetComponent<BossEnemy_Controller>();

            if (target == null)
            {
                Debug.Log("Target is Null");
                yield return null;
            }
            else if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= range)
                {
                    yield return new WaitForSeconds(1.5f);
                    if (target != null)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2);
                        yield return new WaitForSeconds(1.5f);
                        transform.LookAt(target.transform.position);
                    }
                }
            }
            yield return null;
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * projSpeed * Time.deltaTime);
        if (hasSpecialProjectile == true && gameObject != null)
        {
            Vector3 specialPos = transform.position;
            Vector3.MoveTowards(target.transform.position, specialPos.normalized, 2);
        }
        if (transform.position.y > 5)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 2);
            }
            Destroy(gameObject, 2);
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

    public void AssignEnemyProjectile()
    {
        isEnemyProjectile = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isEnemyProjectile == true)
        {
            PlayerController player = collision.transform.GetComponent<PlayerController>();

            if (player != null)
            {
                player.SendMessage("TakeDamage");
                return;
            }
        }
        else if(collision.gameObject.tag == "Player")
        {
            hasSpecialProjectile = true; 
        }
    }
}
