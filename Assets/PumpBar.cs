using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PumpBar : MonoBehaviour
{
    public AudioSource audioSource;
    public Image waterBar;
    public ParticleSystem WaterEffect;
    public AudioClip waterFalling;
    public AudioClip pumpUp;
    public Transform water;
    public ChatBubble chatBubble;
    public RiceField riceField;

    float maxDegree=0;
    bool pumpUpPlayed = false;
    Vector3 oldWaterPosition;


    // Start is called before the first frame update
    void Start()
    {
        oldWaterPosition = water.position;
    }

    // Update is called once per frame
    void Update()
    {
        // pump water
        if (transform.eulerAngles.x > 13 && transform.eulerAngles.x <= 180 )
        {
            if (transform.eulerAngles.x >= maxDegree)
            {
                maxDegree = transform.eulerAngles.x;
            }
            //playSound
            if (transform.eulerAngles.x >= 21)
            {
                if (pumpUpPlayed == false)
                {
                    pumpUpPlayed = true;
                    audioSource.PlayOneShot(pumpUp);
                }
            }
        }

        // increase fill Amount
        if (transform.eulerAngles.x <= 13 && maxDegree > 20)
        {
            if (waterBar.fillAmount < 1.0f)
            {
                water.transform.position = new Vector3( water.transform.position.x, water.transform.position.y + 0.01f, water.transform.position.z);
                chatBubble.UpdateChatBubbles(false);
                waterBar.fillAmount += (maxDegree*2)/1000;
            }
            maxDegree = 0;
            WaterEffect.Play();
            audioSource.PlayOneShot(waterFalling);
            pumpUpPlayed = false;
        }
        

    }

    public void ResetWaterPosition()
    {
        water.position = oldWaterPosition;
        chatBubble.ResetChatBubbles();
    }

    public void UpdateAutoWaterSystem()
    {
       
        water.transform.position = new Vector3( water.transform.position.x, water.transform.position.y + 0.03f, water.transform.position.z);
        chatBubble.UpdateChatBubbles(true);
        waterBar.fillAmount += 0.2f;
    }
    
}
