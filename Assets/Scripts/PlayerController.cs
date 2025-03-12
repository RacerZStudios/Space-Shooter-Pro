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
    [SerializeField] 
    private Transform playerT;
    [SerializeField]
    public float projSpeed = 10;
    [SerializeField]
    private float fireTime = 0.5f;
    private float canFire = -1f;
    private float standardFire = 5; 
    [SerializeField] 
    private int lives = 3;
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

    [SerializeField]
    public bool isController;

    [SerializeField]
    private bool thurstSliderActive; 

    private void Start()
    {
        if(player)
        {
            GameObject.FindObjectOfType<PlayerController>();
            player = this.gameObject;
            SetStartPlayerLocation();
        }

        // set player speed 
        speed = 4.5f; 

        if(isSpecialProjectile == true)
        {
            isSpecialProjectile = false; 
        }

        if(isNegativeEffect == true)
        {
            isNegativeEffect = false;
        }

        ammoAmount = 15; 

        if(uI_Manager)
        {
            thurstSliderActive = true; 
            if(thurstSliderActive == true)
            {
                uI_Manager = FindObjectOfType<UI_Manager>();
            }
        }
        else
        {
            thurstSliderActive =false;
            uI_Manager = null;
        }

        if(spawnManager != null)
        {
            spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        }

        audioSource = GetComponent<AudioSource>();
        spriteRender = GetComponent<SpriteRenderer>(); 

        if (spawnManager == null)
        {
            return; 
        }

        if(audioSource == null)
        {
            return;  
        }
        else
        {
            audioSource.clip = projSound; 
        }
    }
    void SetStartPlayerLocation()
    {
        // take the current player position and assign a new position of (0,0,0) 
        playerT.position = new Vector3(0, 0, 0);
        
    }

    void SetNewPlayerLocation()
    {
        if (playerT.position.y < -10.0f && player != null)
        {
            // get and set player controller position 
            player.transform.position = new Vector3(0, 0, 0);
        }
    }

    // Controller Input 
    void ControllerInput()
    {
        if(horizontalInput > 0 || verticalInput > 0 || horizontalInput < 0 || verticalInput < 0)
        {
            isController = true;
            if (Input.GetJoystickNames().Length > 0 && isController == true)
            {
                if (Input.GetButtonDown("Fire"))
                {
                    if(ammoAmount > 0)
                    {
                        FireProjectile();

                        if (isTrippleShot)
                        {
                            FireTripleShotProjectile();
                        }
                        else if (isEMPProjectile)
                        {
                            FireEMPProjectile();
                        }
                        else if (isSpecialProjectile)
                        {
                            FireSpecialProjectile();
                        }
                    }
                   
                }
                else if (Input.GetButton("Thruster") && isController == true)
                {
                    thurstSliderActive = true;
                    if(thurstSliderActive.Equals(true))
                    {
                        ThrustActive();
                    }
                }
                else if (Input.GetButton("ResetPlayer") && isController == true)
                {
                    ResetPlayerPos();
                }

                isController = false;
            }
            return; 
        }       
    }

    void FireProjectile()
    {
        // Player Projectile Input 
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire && ammoAmount >= 0 && ammoAmount != 0 || isController == true)
        {
            canFire = Time.time + fireTime;
            if (audioSource != null)
            {
                audioSource.Play();
            }
            Instantiate(playerProjectile, projT.position + new Vector3(0, projOffset.y, 0), Quaternion.identity);
            playerProjectile.GetComponent<Rigidbody2D>().AddForce(projT.transform.position * projSpeed * Time.deltaTime);
            if (uI_Manager != null)
            {
                uI_Manager.AmmoStorage(ammoAmount);
                ammoAmount--;
                if(ammoAmount <= 0)
                {
                    ammoAmount = 0;
                }
            }
        }
    }

    void FireTripleShotProjectile()
    {
        // Triple Shot Projectile 
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= standardFire && isTrippleShot == true && ammoAmount >= 0 && ammoAmount != 0 || isController == true)
        {
            canFire = Time.time + fireTime;
            if (isTrippleShot == true)
            {
                Instantiate(trippleShotObject, projT.position + new Vector3(0, projOffset.y, 0), Quaternion.identity);
                if (uI_Manager != null)
                {
                    uI_Manager.AmmoStorage(ammoAmount);
                    ammoAmount--;
                    if (ammoAmount <= 0)
                    {
                        ammoAmount = 0;
                    }
                }
            }
            isTrippleShot = false;
        }
    }

    void FireEMPProjectile()
    {
        // EMP Projectile 
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= standardFire && isEMPProjectile == true && ammoAmount >= 0 && ammoAmount != 0 || isController == true)
        {
            standardFire = Time.time + fireTime;
            if (isEMPProjectile == true)
            {
                Instantiate(empProjectile, projT.position + new Vector3(0, projOffset.y, 0), Quaternion.identity);
                if (uI_Manager != null)
                {
                    uI_Manager.AmmoStorage(ammoAmount);
                    ammoAmount--;
                    if (ammoAmount <= 0)
                    {
                        ammoAmount = 0;
                    }
                }
            }
            isEMPProjectile = false;
        }
    }

    void FireSpecialProjectile()
    {
        // Special Projectile 
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= standardFire && isSpecialProjectile == true && ammoAmount >= 0 && ammoAmount != 0 || isController == true)
        {
            standardFire = Time.time + fireTime;
            if (isSpecialProjectile == true)
            {
                Instantiate(specialProjectile, projT.position + new Vector3(0, projOffset.y, 0), Quaternion.identity);
                if (uI_Manager != null)
                {
                    uI_Manager.AmmoStorage(ammoAmount);
                    ammoAmount--;
                    if (ammoAmount <= 0)
                    {
                        ammoAmount = 0;
                        return;
                    }
                }
            }
            isSpecialProjectile = false;
        }
    }

    private void Update()
    {
        if(Input.GetJoystickNames().Length > 0)
        {
            ControllerInput(); 
        }

        horizontalInput = Input.GetAxis("Horizontal"); 
        verticalInput = Input.GetAxis("Vertical");

        FireProjectile();
        FireTripleShotProjectile();
        FireEMPProjectile();
        FireSpecialProjectile();

        // in line code cleanup 
        // Clamp transform from -3 and 0 
        // playerT.position = new Vector3(playerT.position.x, Mathf.Clamp(playerT.position.y, -3, 0), 0); 

        // Vector3 direction = new Vector3(horizontalInput, verticalInput, 0); // another way to handle axis input 
        // playerT.Translate(direction * speed * Time.deltaTime); 

        Vector3 dir = new Vector3(horizontalInput, verticalInput, 0);

        if (dir.x < 0)
        {   // new Vector3(1,0,0) * variable speed * real timestep 
            playerT.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (dir.x > 0)
        {
            playerT.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (dir.y > 0)
        {
            playerT.Translate(Vector3.up * speed * Time.deltaTime);
        }

        if (dir.y < 0)
        {
            playerT.Translate(Vector3.down * speed * Time.deltaTime);
        }

        ResetPlayerPos();

        SetNewPlayerLocation();

        if (uI_Manager != null)
        {
            if (uI_Manager.thurstSlider.value != 0 && isNegativeEffect != true)
            {
                ThrustActive();
            }
        }

        if(uI_Manager != null)
        {
            if(uI_Manager.thurstSlider.value < 0.1f)
            {
                RegenThrustActive();
            }
        }

        // if player is greater than 0, y position = 0 
        // else if position on y is less than -3.0f, y position = -3.0f
        if (playerT.position.y > 10)
        {
            playerT.position = new Vector3(playerT.position.x, 0, 0); 
        }
        else if(playerT.position.y <= -3.0f)
        {
            playerT.position = new Vector3(playerT.position.x, -3.0f, 0);
        }

        // if player on x > 10, x pos = -10 
        // else if player on x is < -10,  pos = 10 
        if(playerT.position.x > 10)
        {
            playerT.position = new Vector3(-10, playerT.position.y, 0); 
        }
        else if(playerT.position.x < -10)
        {
            playerT.position = new Vector3(10, playerT.position.y, 0); 
        }

        // reset player root transform and position 
        Vector3 playerPos = new Vector3(playerT.transform.position.x, playerT.transform.position.y, 0);
        if(playerT.position.y < -3)
        {
            Debug.Assert(true, playerT.position.y);
            playerT.position = new Vector3(playerPos.x, playerPos.y, 0);
            playerT.position = playerPos;
            playerT.GetChild(0).transform.position = playerPos;
        }
        else
        {
            playerT.position = new Vector3(0, 0, 0);
            playerT.position = playerPos;
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

    public void ResetPlayerPos()
    {
        // reset player position with Key bind 
        if (playerT.position.y < 0 || playerT.position.y > 0 || player != null)
        {
            if (Input.GetKeyDown(KeyCode.T) || isController == true)
            {
                gameObject.transform.position = new Vector3(0, 0, 0);
                return;
            }
        }
    }

    public void ThrustActive()
    {
        // Thrust Speed Feature 
        if (Input.GetKey(KeyCode.LeftShift) && uI_Manager.thurstSlider.value > -0.1f || isController == true)
        {
            if(uI_Manager)
            {
                uI_Manager.StartThrust();
            }
            else
            {
                Debug.Assert(true, "UI Manager is Null", uI_Manager);
                uI_Manager = null;
            }

            if (horizontalInput < 1)
            {
                playerT.Translate(Vector3.left * thrustSpeed * Time.deltaTime);
            }

            else if (horizontalInput > 0)
            {
                playerT.Translate(Vector3.right * thrustSpeed * Time.deltaTime);
            }

            if (verticalInput > 0.1f)
            {
                playerT.Translate(Vector3.up * thrustSpeed * Time.deltaTime);
            }

            else if (verticalInput < -0.1f)
            {
                playerT.Translate(Vector3.down * thrustSpeed * Time.deltaTime);
            }
            return; 
        }
    }

    public void RegenThrustActive()
    {
        if (uI_Manager.thurstSlider.value <= 0.1f && uI_Manager != null)
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
        speed *= speedBoost * 2; 
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
            yield return new WaitForSeconds(2);
            spriteRender.color = new Color(0, 209f, 255f, 255f);
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

    public void AddFinalAmmo(int ammo)
    {
        if(uI_Manager != null)
        {
            if(ammoAmount < 1)
            {
                ammoAmount = ammo + ammoAmount + 30; 
            }
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