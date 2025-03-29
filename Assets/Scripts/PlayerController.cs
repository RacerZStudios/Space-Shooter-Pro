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
    private Animator anim;
    [SerializeField]
    private Animator thrustAnim; 
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
    private float thrustForce = 50; 
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
    public int enemy; 

    [SerializeField]
    private AudioClip projSound;
    [SerializeField]
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

    [SerializeField]
    private WaveSpawn waveSpawn;

    void Awake()
    {
        if (anim.name == "PlayerController")
        {
            anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        }

        if (thrustAnim.name == "Thruster")
        {
            thrustAnim = GameObject.FindGameObjectWithTag("Thruster").GetComponentInChildren<Animator>();
        }
    }

    private void Start()
    {
        score = 0;
        enemy = 0;

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

        if(waveSpawn != null)
        {
            waveSpawn = FindObjectOfType<WaveSpawn>();
        }
        else
        {
            waveSpawn = FindObjectOfType<WaveSpawn>().GetComponent<WaveSpawn>();
        }

        while (waveSpawn != null)
        {
            if (enemy != 10)
            {
                waveSpawn.StartCoroutine(waveSpawn.Wave1());
                if(enemy >= 10)
                {
                    waveSpawn.StopCoroutine(waveSpawn.Wave1());
                }
                break; 
            }
            if (enemy >= 10)
            {
                waveSpawn.StartCoroutine(waveSpawn.Wave2());
                if(enemy >= 20)
                {
                    waveSpawn.StopCoroutine(waveSpawn.Wave2());
                }
                break; 
            }
            if (enemy >= 20)
            {
                waveSpawn.StartCoroutine(waveSpawn.Wave3());
                if(enemy >= 30)
                {
                    waveSpawn.StopCoroutine(waveSpawn.Wave3());
                }
                break; 
            }
            break; 
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

        // restart game if boss is null and doesn't trigger win game after first play through 
        if(ammoAmount <= 0 && uI_Manager.thurstSlider.value <= 0.0f)
        {
            if(gM)
            {
                gM.EndGame();
            }
            // else assert here 
        }

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
            if (uI_Manager.thurstSlider.value != 0 && isNegativeEffect != true || isController)
            {
                ThrustActive();
            }

            HandleThrusterMovement();
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
        if (playerT.position.y > 13)
        {
            playerT.position = new Vector3(0,0, 0); 
        }
        else if(playerT.position.y <= -6.0f)
        {
            playerT.position = new Vector3(0, 0, 0);
        }

        // if player on x > 10, x pos = -10 
        // else if player on x is < -10,  pos = 10 
        if(playerT.position.x > 13)
        {
            playerT.position = new Vector3(playerT.position.y, 0, 0); 
        }
        else if(playerT.position.x < -13)
        {
            playerT.position = new Vector3(playerT.position.y, 0, 0); 
        }

        // reset player root transform and position 
        Vector3 playerPos = new Vector3(playerT.transform.position.x, playerT.transform.position.y, 0);
        if(playerT.position.y < -3)
        {
            playerT.position = new Vector3(playerPos.x, playerPos.y, 0);
            playerT.position = playerPos;
            playerT.GetChild(0).transform.position = playerPos;
        }
        else
        {
            playerT.position = new Vector3(0, 0, 0);
            playerT.position = playerPos;
        }

        if(waveSpawn == null)
        {
            waveSpawn = FindObjectOfType<WaveSpawn>();
            return; 
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

    private void FixedUpdate()
    {
       // ThrusterRigidbody(); 
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
        else
        {
            if(playerT.position.y < 0)
            {
                gameObject.transform.position = new Vector3(0, 0, 0);
            }
        }
    }

    private void HandleThrusterMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isThruster = true;
            // ThrusterTransform();
            // ThrusterTranslate();
        }
    }

    private void ThrusterRigidbody() 
    {
        // Rigidbody method with velocity | FixedUpdate

        // if the W key and isThruster boolean is true, apply rigidbody force in the up direction
        if (Input.GetKey(KeyCode.W) && isThruster.Equals(true))
        {
            Vector3 moveUp = transform.up;
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(moveUp * thrustSpeed * thrustForce * Time.deltaTime, ForceMode2D.Force);
            Debug.Log(isThruster.ToString() + moveUp);
        }

        // if the D key and isThruster boolean is true, apply rigidbody force in the right direction
        if (Input.GetKey(KeyCode.D) && isThruster.Equals(true))
        {
            Vector3 moveRight = transform.right;
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(moveRight * thrustSpeed * thrustForce * Time.deltaTime, ForceMode2D.Force);
            Debug.Log(isThruster.ToString() + moveRight);
        }

        // if the S key and isThruster boolean is true, apply rigidbody force in the down direction
        // we inverse the transform.up axis from + to - for down position
        if (Input.GetKey(KeyCode.S) && isThruster.Equals(true))
        {
            Vector3 moveDown = -transform.up;
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(moveDown * thrustSpeed * thrustForce * Time.deltaTime, ForceMode2D.Force);
            Debug.Log(isThruster.ToString() + moveDown);
        }

        // if the A key and isThruster boolean is true, apply rigidbody force in the left direction
        // we inverse the transform.right axis from + to - for left position
        if (Input.GetKey(KeyCode.A) && isThruster.Equals(true))
        {
            Vector3 moveLeft = -transform.right;
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(moveLeft * thrustSpeed * thrustForce * Time.deltaTime, ForceMode2D.Force);
            Debug.Log(isThruster.ToString() + moveLeft);
        }
    }

    private void ThrusterTransform()
    {
        if (Input.GetKey(KeyCode.W) && isThruster.Equals(true))
        {
            // transform method up direction 
            transform.position += Vector3.up * thrustSpeed * Time.deltaTime;
            Debug.Log(Vector3.up);
        }
        if (Input.GetKey(KeyCode.D) && isThruster.Equals(true))
        {
            // transform method right direction 
            transform.position += Vector3.right * thrustSpeed * Time.deltaTime;
            Debug.Log(Vector3.right);
        }
        if (Input.GetKey(KeyCode.S) && isThruster.Equals(true))
        {
            // transform method down direction
            transform.position += Vector3.down * thrustSpeed * Time.deltaTime;
            Debug.Log(Vector3.down);
        }
        if (Input.GetKey(KeyCode.A) && isThruster.Equals(true))
        {
            // transform method left firection 
            transform.position += Vector3.left * thrustSpeed * Time.deltaTime;
            Debug.Log(Vector3.left);
        }
    }

    private void ThrusterTranslate()
    {
        if (Input.GetKey(KeyCode.W) && isThruster.Equals(true))
        {
            transform.Translate(Vector3.up * thrustSpeed * Time.deltaTime); 
        }
        if (Input.GetKey(KeyCode.D) && isThruster.Equals(true))
        {
            transform.Translate(Vector3.right * thrustSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) && isThruster.Equals(true))
        {
            transform.Translate(Vector3.down * thrustSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) && isThruster.Equals(true))
        {
            transform.Translate(Vector3.left * thrustSpeed * Time.deltaTime);
        }
    }

    public void ThrustActive() // Thrust for Player movement stat 
    {
        if (Input.GetKey(KeyCode.LeftShift) && uI_Manager.thurstSlider.value > -0.1f || isController == true)
        {
            if (uI_Manager)
            {
                uI_Manager.StartThrust();
                thrustAnim.Play("Thrust"); // Thrust animation visuals 
                thrustAnim.SetBool("Thrusting", true);
            }
            else
            {
                Debug.Assert(true, "UI Manager is Null", uI_Manager);
                uI_Manager = null;
            }

            if (horizontalInput < 1.0f && horizontalInput < 0)
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
        else if (Input.GetKeyUp(KeyCode.LeftShift) && uI_Manager.thurstSlider.value > -0.1f || isController == true)
        {
            thrustAnim.SetBool("Thrusting", false); // deactivate Thurst animation visual when not thrusting 
            thrustAnim.Play("Idle");
        }
    }

    public void RegenThrustActive()
    {
        if (uI_Manager.thurstSlider.value <= 0.1f && uI_Manager != null)
        {
            uI_Manager.StartCoroutine(uI_Manager.RegenThrust()); 
            thrustAnim.SetBool("Thrusting", false);
            thrustAnim.Play("Idle");
        }
        else if (uI_Manager == null)
        {
            StopAllCoroutines();
        }
    }

    public void TakeDamage()
    {
        audioSource.Play();

        if(isShield.Equals(true))
        {
           // if shield is active take shield damage by 1 per shield life available (3). 
            shield_Manager.shieldLife -= 1; 
            if (shield_Manager.shieldLife <= 0)
            {
                isShield = false;
                shieldVis.SetActive(false);
            }
        }

        if(gameObject != null && uI_Manager != null && shield_Manager.shieldLife < 1)
        {
            uI_Manager.UpdateLives(lives);
            lives--;
           // print(lives); 
            if(lives >= 2)
            {
                engineFire[0].gameObject.SetActive(true); 
            }
            else if (lives >= 1)
            {
                engineFire[1].gameObject.SetActive(true); 
            }

            if(lives < 1)
            {
                lives = 0; 
               // print(lives);
                gM.isGameOver = true;
               // print(gM.isGameOver);
                spawnManager.PlayerDead();
                // player dead 
                if (anim)
                {
                    anim.SetTrigger("PlayerDeath");
                    uI_Manager.PlayerDestroyed(isPlayer: true);
                    engineFire[0].gameObject.SetActive(false);
                    engineFire[1].gameObject.SetActive(false); 
                    Destroy(player, 2);
                    Destroy(gameObject, 2);
                }
                else
                {
                    anim = null;
                }
            }
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

    public void SpeedBoostActive() // Increase player speed when speed boost picked up 
    {
        if (uI_Manager.thurstSlider.value > 0)
        {
            isSpeedBoost = true;
            speed *= speedBoost + 0.85f;
            StartCoroutine(SpeedBoostPowerDown());
        }
    }

    public IEnumerator SpeedBoostPowerDown() // decrease player speed after 5 seconds // To Do: Add visual feedback 
    {
        yield return new WaitForSeconds(5);
        isSpeedBoost = false;
        speed /= speedBoost; 
    }

    public void ShieldActive()
    {
        if(shield_Manager != null || shieldVis != null)
        {
            shield_Manager.shieldLife = 3; 
            isShield = true;
            shieldVis.SetActive(true);
           // print(shield_Manager.shieldLife);
            switch (shield_Manager.shieldLife)
            {
                case 0:
                    shield_Manager.shieldLife = 3;
                    break;
                case 1:
                    shield_Manager.shieldLife = 2;
                    break;
                case 2:
                    shield_Manager.shieldLife = 1;
                    break;
                default:
                    break;
            }

            ShieldPowerDown();
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
            if(shield_Manager.shieldLife > 0)
            {
                shield_Manager.shieldLife--;

                if (shield_Manager.shieldLife < 1)
                {
                    shield_Manager.shieldLife = 0;
                    if(shield_Manager.shieldLife <= 0)
                    {
                        isShield = false;
                        shieldVis.SetActive(false);
                    }
                }
            }
            else if (shield_Manager.shieldLife <= 0)
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
        uI_Manager.AddLives(1); 
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
            ammoAmount = ammo + ammoAmount;
            uI_Manager.AmmoStorage(ammoAmount);
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

    public void AddEnemiesDefeated(int count)
    {
        if(uI_Manager != null)
        {
            enemy += count;
            uI_Manager.UpdateEnemiesDefeated(enemy);
            if (waveSpawn != null)
            {
                waveSpawn.EnemiesDestroyed(enemies: 1);
            }
        }
    }

    public void AddScore(int points)
    {
        if(uI_Manager != null)
        {
            score += points;
            uI_Manager.UpdateScore(score);
        }
    }

    public void UpdateWaves(int waves)
    {
        if (uI_Manager != null)
        {
            enemy += waves;
            uI_Manager.WaveSpawn(enemy);
            if (waveSpawn != null)
            {
                waveSpawn.Wave.CompareTo(waves);
            }
        }
    }
}