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
    [SerializeField] Transform playerT;
    [SerializeField]
    public float projSpeed = 10;
    [SerializeField]
    private float fireTime = 0.5f;
    private float canFire = -1f; 

    private float horizontalInput = -0.1f;
    private float verticalInput = 0.1f;

    public GameObject playerProjectile;
    public Transform projT;
    public Vector3 projOffset = new Vector3(0, 0.5f, 0);

    private void Start()
    {
        OnStartPlayerLocation(); 
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
            Instantiate(playerProjectile, projT.position + new Vector3(0, projOffset.y,0), Quaternion.identity);
            playerProjectile.GetComponent<Rigidbody>().AddForce(projT.transform.position * projSpeed * Time.deltaTime); 
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

        // if player is greater than 0, y position = 0 
        // else if position on y is less than -3.0f, y position = -3.0f
        if(playerT.position.y >= 0)
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
}
