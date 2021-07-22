using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

public class Boss_Health : MonoBehaviour
{
    [SerializeField]
    public int minhealth;
    [SerializeField]
    private int maxhealth;
    [SerializeField]
    private int currenthealth;
    [SerializeField]
    private Slider slider; 

    private void Start()
    {
        if(slider == null)
        {
            Debug.Log("Slider is Null"); 
        }
        slider = FindObjectOfType<Slider>().GetComponentInChildren<Slider>(); 
        minhealth = 0;
        currenthealth = 100;
        maxhealth = 100;
        maxhealth = currenthealth;  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerProjectile")
        {
            currenthealth -= 10;
            slider.value -= 0.1f; 
            if(currenthealth <= 10 && slider.value <= 0.1f)
            {
                Destroy(gameObject); 
            }
        }
    }
}
