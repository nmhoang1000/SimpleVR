using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpSoundTrigger : MonoBehaviour
{
    public AudioClip sound;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "Handler")
        {
            audioSource.PlayOneShot(sound);
        }
    }
}
