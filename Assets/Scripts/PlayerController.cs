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
    private float thrustSoeed; 

    private float horizontalInput = -0.1f;
    private float verticalInput = 0.1f;

    public GameObject playerProjectile;
    public Transform projT;
    public Vector3 projOffset = new Vector3(0, 0.5f, 0);

    [SerializeField]
    private UI_Manager uI_Manager; 

    public GameObject trippleShotObject;

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

    private void Start()
    {
        uI_Manager = GameObject.Find("Canvas").GetComponent<UI_Manager>(); 
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<GameObject>(); 
        OnStartPlayerLocation(); 

        if(spawnManager == null)
        {
            Debug.LogError("The SpawnManager doesn't exist and is null");
            return; 
        }

        if(uI_Manager == null)
        {
            Debug.LogError("UI Manager is null");
            return; 
        }

        if(audioSource == null)
        {
            Debug.LogError("No audio source is assigned or is null"); 
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
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
        {
            canFire = Time.time + fireTime;
            audioSource.Play(); 
            Instantiate(playerProjectile, projT.position + new Vector3(0, projOffset.y,0), Quaternion.identity);
            playerProjectile.GetComponent<Rigidbody2D>().AddForce(projT.transform.position * projSpeed * Time.deltaTime); 

            if(isTrippleShot == true)
            {
                Instantiate(trippleShotObject, projT.position + new Vector3(0, projOffset.y, 0), Quaternion.identity);
                return; 
            }
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

        // Thrust Speed Feature 
        if(Input.GetKey(KeyCode.LeftShift))
        {

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
    }

    public void TakeDamage()
    {
        if(isShield.Equals(true))
        {
            isShield = false;
            shieldVis.SetActive(false); 
            return; 
        }

        if(gameObject != null)
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

        if(lives <= 0 || lives <= 1)
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
        TrippleShotActive();
        isTrippleShot = false; 
    }

    public void SpeedBoostActive()
    {
        isSpeedBoost = true;
        speed *= speedBoost; 
        StartCoroutine(SpeedBoostPowerDown()); 
    }

    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5);
        isSpeedBoost = false;
        speed /= speedBoost; 
    }

    public void ShieldActive()
    {
        isShield = true;
        shieldVis.SetActive(true);
        StartCoroutine(ShieldPowerDown()); 
    }

    IEnumerator ShieldPowerDown()
    {
        yield return new WaitForSeconds(3);
        isShield = false;
        shieldVis.SetActive(false);
    }

    public void AddScore(int points)
    {
        uI_Manager.UpdateScore(score);
        score += points;
    }
}
