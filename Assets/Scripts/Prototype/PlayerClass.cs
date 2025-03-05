using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    // Veriables 
    [SerializeField] // stores data and is editable in inspector
    private float speed;

    // create a Fire method to spawn projectile 
    // create a spawn position for projectile to spawn out from 
    [SerializeField]
    private Transform playerLazerPos;

    [SerializeField]
    private GameObject playerLazer;

    [SerializeField]
    private GameObject tripleShotLazer;

    [SerializeField]
    private bool hasTripleShot = false;

    [SerializeField]
    private bool hasShield = false; 

    // create a fire rate system 
    [SerializeField]
    private float _fireRate;

    // when can we fire next? 
    [SerializeField]
    private float canFire = -1; // create a timestep override 

    [SerializeField]
    // create a lives system 
    private int lives = 3;

    [SerializeField]
    private int score; 

    // set spawn manager reference // get spawn system 
    [SerializeField]
    private SpawnSystem spawnSystem;

    // create a unique player material effect for powerUps 
    [SerializeField]
    private SpriteRenderer playerMaterial;

    [SerializeField]
    private UIManagerClass uiManager; 

    // Start is called before the first frame update
    void Start()
    {
        playerMaterial = GetComponentInChildren<SpriteRenderer>();

        spawnSystem = GameObject.FindObjectOfType<SpawnSystem>();

        uiManager = GameObject.FindObjectOfType<UIManagerClass>();

        _fireRate = 0.5f; 
        // set start speed
        speed = 5; 

        // set default player start position
        transform.position = new Vector3(0, 0, 0); 
    }

    private void Fire()
    {
        // check if has Triple shot 
        if(hasTripleShot == false)
        {
            // spawn reqular single shot 
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
            {
                // get current fire rate 
                canFire = Time.time + _fireRate;

                // spawn projectile at transform position 
                // create an instiation method 
                Instantiate(playerLazer, playerLazerPos.transform);
            }
        }
        else
        {
            if(hasTripleShot == true)
            {
                if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
                {
                    canFire = Time.time + _fireRate;

                    Instantiate(tripleShotLazer, playerLazerPos.transform); 
                }
            }
        }
       
    }


    // Update is called once per frame
    void Update()
    {
        // Set single or triple shot 
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            hasTripleShot = false; 
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            hasTripleShot = true;
        }

        // Call Fire method 
        Fire(); 

        // create movement 
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical"); 

        // move on left and right (x) axis [Horizontal Movement] 
        if( h > 0)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime); 
        }
        else
        {
            if(h < 0)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }

        // move on the up and down (y) axis [Vertical Movement]
        if (v > 0)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else
        {
            if (v < 0)
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
        }

        // we want to restrict movement bouds 
        // if player position is > y or x axis, reset position 
        // get the direction vector of player position 
        Vector3 direction = transform.position; 
        if((int)direction.y > 8) // get max position delta in y positive direction
        {
            // reset direction to normal position 
            transform.position = new Vector3(0, (int)direction.y - 8, 0); 
        }
        else
        {
            if((int)direction.y < -8)
            {
                transform.position = new Vector3(0, (int)direction.y + 8, 0);
            }
        }
        if((int)direction.x > 8)
        {
            transform.position = new Vector3(-(int)direction.x - 8, 0);
        }
        else
        {
            if((int)direction.x < -8)
            {
                transform.position = new Vector3((int)direction.x + 8, 0); 
            }
        }
    }

    public void TakeDamage()
    {
        if (hasShield.Equals(false))
        {
            Debug.Log("Player taking damage");
            playerMaterial.color = Color.red;
            // decrement lives if damaged 
            lives--;
            PlayerLives(lives); 

            // check if player lives is < 1 or player is dead 
            if (lives < 1)
            {
                // stop spawn coroutine if player is dead 
                // get reference to spawn manager // spawn system 
                spawnSystem.IsPlayerDead();
                lives = 0;
                if (lives == 0)
                {
                    // destroy player and respawn or end game 
                    Destroy(gameObject);
                }
            }

            // negate damage to player if has shield is true 
            // return 
        }
        else if (hasShield.Equals(true))
        {
            lives = lives + 0;
            Debug.Log("Reflect Damage");
            playerMaterial.color = Color.magenta; 
        }
        else
        {
            playerMaterial.color = Color.cyan; // set to original color after shield life has ended 
        }
       
    }

    public void PlayerLives(int health)
    {
        lives = health;
        uiManager.UpdateLives(health);
    }

    // create AddScore method 
    public void AddScore(int points)
    {
        // get ui manager class to score text 
        // create an int variable for score 
       // points = Random.Range(0, points);
        score += points; 
        // pass in update player score 
        uiManager.UpdatePlayerScore(score);
    }

    public void GetShield()
    {
        hasShield = true;
        playerMaterial.color = Color.cyan;
        StartCoroutine(ShieldActive());
    }

    private IEnumerator ShieldActive()
    {
        playerMaterial.color = Color.blue;
        yield return new WaitForSeconds(3); 
        hasShield = false;
        playerMaterial.color = Color.cyan;
    }

    public void SpeedBoost()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity *= 5.5f * Time.deltaTime;
        speed *= 2;
        playerMaterial.color = Color.blue; 
        StartCoroutine(SetSpeedBoostActive());
    }

    private IEnumerator SetSpeedBoostActive()
    {
        yield return new WaitForSeconds(3);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity /= 0.5f - Time.deltaTime;
        playerMaterial.color = Color.cyan;
        // return to normal speed
        speed = 5; 
    }

    public void TripleShot()
    {
        hasTripleShot = true;
        StartCoroutine(SetTripleShotActive());
    }

    private IEnumerator SetTripleShotActive()
    {
        yield return new WaitForSeconds(3);
        hasTripleShot = false;
        playerMaterial.color = Color.cyan;
    }
}
