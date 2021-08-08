using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <Begin_variables_summary>
    /// Variables overview 
    /// *** public or private reference ***
    /// ** data types (int, float, bool, string) ** 
    /// each variable has a name 
    /// optional value assigned 
    /// </End_variables_summary>
 
    // SerializeField data type, serialized the data so we can read it within the inspector
    // additionally SerializeField is handy for simplifying reference when having to use GetComponent 
    [SerializeField] 
    public float speed = 3;
    [SerializeField]
    public float speedBoost = 2; 
    [SerializeField] Transform playerT;
    [SerializeField]
    public float projSpeed = 10;
    [SerializeField]
    private float fireTime = 0.5f;
    private float canFire = -1f;
    private float standardFire = 5; 
    [SerializeField] private int lives = 3;
    [SerializeField]
    private SpawnManager spawnManager;
    [SerializeField] // toggle behavior active in inspector 
    public bool isTrippleShot;
    [SerializeField]
    public bool isSpeedBoost;
    [SerializeField]
    public bool isShield;
    [SerializeField]
    private float thrustSpeed = 5;
    [SerializeField]
    private bool isThruster;
    [SerializeField]
    private bool isEMPProjectile;
    [SerializeField]
    private bool isNegativeEffect;
    [SerializeField]
    private bool isSpecialProjectile; 

    private float horizontalInput = -0.1f;
    private float verticalInput = 0.1f;

    public GameObject playerProjectile;
    public GameObject empProjectile; 
    public Transform projT;
    public Vector3 projOffset = new Vector3(0, 0.5f, 0);

    [SerializeField]
    private UI_Manager uI_Manager; 

    public GameObject trippleShotObject;
    public GameObject specialProjectile; 

    // reference shield visuals 
    public GameObject shieldVis;

    [SerializeField]
    public GameObject[] engineFire; 

    [SerializeField]
    private int score;

    [SerializeField]
    private AudioClip projSound;
    private AudioSource audioSource;
    private GameObject player;
    [SerializeField]
    private GameManager gM;
    [SerializeField]
    private Shield_Manager shield_Manager;
    private int ammoAmount;
    private SpriteRenderer spriteRender; 

    private void Start()
    {
        if(isSpecialProjectile == true)
        {
            isSpecialProjectile = false; 
        }

        if(isNegativeEffect == true)
        {
            isNegativeEffect = false;
        }

        ammoAmount = 15; 

        if(uI_Manager != null)
        {
            uI_Manager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        }

        if(spawnManager != null)
        {
            spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        }

        audioSource = GetComponent<AudioSource>();
        spriteRender = GetComponent<SpriteRenderer>(); 
        player = GetComponent<GameObject>(); 
        OnStartPlayerLocation();

        if (spawnManager == null)
        {
           // Debug.LogError("The SpawnManager doesn't exist and is null");
            return; 
        }

        if(uI_Manager == null)
        {
           // Debug.LogError("UI Manager is null");
            return; 
        }

        if(audioSource == null)
        {
           // Debug.LogError("No audio source is assigned or is null"); 
        }
        else
        {
            audioSource.clip = projSound; 
        }
    }
    void OnStartPlayerLocation()
    {
        // take the current player position and assign a new position of (0,0,0) 
        playerT.position = new Vector3(0, 0, 0); 
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); 
        verticalInput = Input.GetAxis("Vertical");

        // Player Projectile Input 
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > canFire && isEMPProjectile == false && ammoAmount >= 0 && ammoAmount != 0)
        {
            canFire = Time.time + fireTime;
            if(audioSource != null)
            {
                audioSource.Play();
            }
            Instantiate(playerProjectile, projT.position + new Vector3(0, projOffset.y,0), Quaternion.identity);
            playerProjectile.GetComponent<Rigidbody2D>().AddForce(projT.transform.position * projSpeed * Time.deltaTime);
            if(uI_Manager != null)
            {
                uI_Manager.AmmoStorage(ammoAmount);
                ammoAmount--;
            }

            // Triple Shot Projectile 
            if (isTrippleShot == true)
            {
                Instantiate(trippleShotObject, projT.position + new Vector3(0, projOffset.y, 0), Quaternion.identity);
                if(uI_Manager != null)
                {
                    uI_Manager.AmmoStorage(ammoAmount);
                    return;
                }
            }
        }

        // EMP Projectile 
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= standardFire && isEMPProjectile == true && ammoAmount >= 0 && ammoAmount != 0)
        {
            standardFire = Time.time + fireTime;
            if (isEMPProjectile == true)
            {
                Instantiate(empProjectile, projT.position + new Vector3(0, projOffset.y, 0), Quaternion.identity);
                uI_Manager.AmmoStorage(ammoAmount);
            }
            isEMPProjectile = false; 
        }

        // Special Projectile 
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= standardFire && isSpecialProjectile == true && ammoAmount >= 0 && ammoAmount != 0)
        {
            standardFire = Time.time + fireTime;
            if (isSpecialProjectile == true)
            {
                Instantiate(specialProjectile, projT.position + new Vector3(0, projOffset.y, 0), Quaternion.identity);
                uI_Manager.AmmoStorage(ammoAmount);
            }
            isSpecialProjectile = false;
        }


        // in line code cleanup 
        // Clamp transform from -3 and 0 
        // playerT.position = new Vector3(playerT.position.x, Mathf.Clamp(playerT.position.y, -3, 0), 0); 

        // Vector3 direction = new Vector3(horizontalInput, verticalInput, 0); // another way to handle axis input 
        // playerT.Translate(direction * speed * Time.deltaTime); 

        if (horizontalInput < 0)
        {   // new Vector3(1,0,0) * variable speed * real timestep 
            playerT.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (horizontalInput > 0)
        {
            playerT.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (verticalInput > 0)
        {
            playerT.Translate(Vector3.up * speed * Time.deltaTime);
        }

        if (verticalInput < 0)
        {
            playerT.Translate(Vector3.down * speed * Time.deltaTime);
        }

        Vector3 dir = new Vector3(horizontalInput, verticalInput, 0);

        if(uI_Manager != null)
        {
            if (uI_Manager.thurstSlider.value != 0 && isNegativeEffect != true)
            {
                ThrustActive();
            }
        }

        if(uI_Manager != null)
        {
            if(uI_Manager.thurstSlider.value != -0.1f)
            {
                RegenThrustActive();
            }
        }

        // if player is greater than 0, y position = 0 
        // else if position on y is less than -3.0f, y position = -3.0f
        if (playerT.position.y >= 0)
        {
            playerT.position = new Vector3(playerT.position.x, 0, 0); 
        }
        else if(playerT.position.y <= -3.0f)
        {
            playerT.position = new Vector3(playerT.position.x, -3.0f, 0);
        }

        // if player on x > 10, x pos = -11 
        // else if player on x is < -11,  pos = 11 
        if(playerT.position.x > 10)
        {
            playerT.position = new Vector3(-10, playerT.position.y, 0); 
        }
        else if(playerT.position.x < -10)
        {
            playerT.position = new Vector3(10, playerT.position.y, 0); 
        }

        if (uI_Manager == null)
        {
            Destroy(uI_Manager);
            return;
        }

        if (spawnManager == null)
        {
            Destroy(spawnManager);
            return;
        }
    }

    public void ThrustActive()
    {
        // Thrust Speed Feature 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            uI_Manager.StartThrust();

            if (horizontalInput < 0)
            {
                playerT.TransformDirection(Vector3.left * thrustSpeed * Time.deltaTime);
            }

            if (horizontalInput > 0)
            {
                playerT.Translate(Vector3.right * thrustSpeed * Time.deltaTime);
            }

            if (verticalInput > -0.1f)
            {
                playerT.Translate(Vector3.up * thrustSpeed * Time.deltaTime);
            }

            if (verticalInput < -1)
            {
                playerT.TransformDirection(Vector3.down * thrustSpeed * Time.deltaTime);
            }
        }
    }

    public void RegenThrustActive()
    {
        if (uI_Manager.thurstSlider.value <= 0.5f && uI_Manager != null)
        {
            uI_Manager.StartCoroutine(uI_Manager.RegenThrust());
        }
        else if (uI_Manager == null)
        {
            StopAllCoroutines();
        }
    }

    public void TakeDamage()
    {
        if(isShield.Equals(true))
        {
            isShield = false;
            shieldVis.SetActive(false); 
            return; 
        }

        if(gameObject != null && uI_Manager != null)
        {
            uI_Manager.UpdateLives(lives);
            lives--;

            if(lives == 2)
            {
                engineFire[0].gameObject.SetActive(true); 
            }
            else if (lives == 1)
            {
                engineFire[1].gameObject.SetActive(true); 
            }
        }

        if(lives <= 0)
        {
            gM.isGameOver = true; 
            spawnManager.PlayerDead();
            Destroy(player); 
            Destroy(gameObject); 
        }
    }

    public void TrippleShotActive()
    {
        isTrippleShot = true;
        StartCoroutine(DoTrippleShot()); 
    }  

    public IEnumerator DoTrippleShot()
    {
        yield return new WaitForSeconds(5);
        isTrippleShot = false; 
    }

    public void SpecialProjectileActive()
    {
        isSpecialProjectile = true;      
        StartCoroutine(DoTrippleShot());
    }

    public IEnumerator DoSpecialProjectile()
    {
        yield return new WaitForSeconds(5);
        isSpecialProjectile = false;
    }

    public void EMPPowerUpActive()
    {
        isEMPProjectile = true;
        StartCoroutine(DoTrippleShot());
    }

    public IEnumerator DoEMPShot()
    {
        yield return new WaitForSeconds(5);
        isEMPProjectile = false;
    }

    public void SpeedBoostActive()
    {
        isSpeedBoost = true;
        speed *= speedBoost; 
        StartCoroutine(SpeedBoostPowerDown()); 
    }

    public IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5);
        isSpeedBoost = false;
        speed /= speedBoost; 
    }

    public void ShieldActive()
    {
        if(shield_Manager != null || shieldVis != null)
        {
            isShield = true;
            shieldVis.SetActive(true);
            if (shieldVis.activeInHierarchy)
            {
                ShieldPowerDown();
            }
        }
        else
        {
            Debug.LogError("No Shield referenced"); 
        }
    }

    public void ShieldPowerDown()
    {
        if(shield_Manager != null || shieldVis != null)
        {
            shield_Manager.shieldLife = 1;

            if (shield_Manager.shieldLife < 0)
            {
                isShield = false;
                shieldVis.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("No Shield referenced");
        }
    }

    public IEnumerator AddHealthEffect()
    {
        if(spriteRender != null)
        {
            spriteRender.color = new Color(1, 1, 1, 1);
           // Debug.Log("Collected Health " + Color.red);
            yield return new WaitForSeconds(2);
            spriteRender.color = new Color(0, 209f, 255f, 255f);
           // Debug.Log("Orogin Color " + Color.cyan);
            uI_Manager.addHealthText.gameObject.SetActive(false);
        }
    }

    public void AddHealth(int health)
    {
        uI_Manager.addHealthText.gameObject.SetActive(true); 
        StartCoroutine(AddHealthEffect()); 
        uI_Manager.AddLivves(1); 
        if(lives != 0 && lives == 1)
        {
            lives += 1;
        }
    }

    public void NegativeEffect(int speed)
    {
        PlayerController player = GetComponent<PlayerController>();
        if(player != null)
        {
            player.speed = 0;
            player.transform.position.Normalize();
            isNegativeEffect = true;
            StartCoroutine(DoNegativeEffect());
        }
    }

    public IEnumerator DoNegativeEffect()
    {
        yield return new WaitForSeconds(5);
        PlayerController player = GetComponent<PlayerController>();
        if (player != null)
        {
            player.speed = 3;
            yield return player.transform.position;
            isNegativeEffect = false;
        }
    }

    public void AddAmmo(int ammo)
    {
        if(uI_Manager != null)
        {
            uI_Manager.AmmoStorage(ammoAmount);
            ammoAmount = ammo + ammoAmount;
        }
    }

    public void AddScore(int points)
    {
        if(uI_Manager != null)
        {
            uI_Manager.UpdateScore(score);
            score += points;
        }
    }
}