using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameUI : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private GameManager gM; 

    private void Start()
    {
        if(anim == null)
        {
            Debug.Log("Wingame Anim is Null"); 
        }

        anim = GetComponent<Animator>();
        anim.Play("Wingame_anim");

        if(gM == null)
        {
            Debug.Log("Game Manager is Null"); 
        }
        gM = GetComponentInParent<GameManager>();

        gM.WinGame(); 
    }
}
