using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using UnityEngine;

public class Boss_Health : MonoBehaviour
{
    [SerializeField]
    private int minhealth;
    [SerializeField]
    private int maxhealth;
    [SerializeField]
    public int currenthealth;
    [SerializeField]
    private Slider slider; 

    private void Start()
    {
        if(slider == null)
        {
            Debug.Log("Slider is Null"); 
        }
        slider = FindObjectOfType<Slider>().GetComponentInChildren<Slider>(); 
        if(minhealth != 0)
        {
            minhealth = 0;
        }
        if(currenthealth != 100)
        {
            currenthealth = 100;
        }
        if (maxhealth != 0)
        {
            maxhealth = 100;
        }

        maxhealth = currenthealth;  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerProjectile")
        {
            currenthealth -= 10;
            slider.value -= 0.1f; 
            if(currenthealth <= 1 && slider.value <= 0.1f)
            {
                Destroy(gameObject); 
            }
        }
    }
}
