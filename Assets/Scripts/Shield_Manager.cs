using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Manager : MonoBehaviour
{
    public int shieldLife; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && gameObject.activeInHierarchy)
        {
           // Debug.Log("Shield Hit");
            switch (shieldLife)
            {
                case 0:
                    ShieldColorLight();
                    break;
                case 1:
                    ShieldColorDark();
                    break;
                case 3:
                    ShieldColorNull();
                    break;
                default:
                    break;
            }
        }
    }

    private void ShieldColorLight()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color (0, 4, 0, 1);
    }

    private void ShieldColorDark()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 8, 0, 0.5f);
    }
    private void ShieldColorNull()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color (0, 0, 0, 0);
    }
}