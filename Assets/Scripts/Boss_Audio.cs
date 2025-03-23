using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Audio : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource.playOnAwake = true; 
    }

    private void Start()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
        _audioSource = FindObjectOfType<AudioSource>();
    }

    void Update()
    {
        Destroy(gameObject, 3); 
    }
}
