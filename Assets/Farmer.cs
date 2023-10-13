using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Farmer : MonoBehaviour
{
    public PumpBar pumpbar;
    public RiceField riceField;
    
    public float startTimer;
    public float gameoverTimer;
    public Transform teleportPoint;

    public Transform startCanvas;
    public Transform helperCanvas;
    public Transform buycanvas;
    public Transform gameOverText;
    public Transform landSubsideText;
    public Transform infoText;
    public Transform readyText;

    public AudioSource audioSource;
    public AudioSource timerAudioSource;
    public AudioSource landAudioSource;
    public AudioClip select;
    public AudioClip reward;
    public AudioClip over;
    public AudioClip lose;

    public Transform readyButton;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI floatingText;
    public int moneyAmount;
    public int autoSystemCost;
    public int firstWaterTimes;
    public int secondWaterTimes;
    
    public Transform autoHandPumps;
    public Transform manualHandPump;
    public Transform mudField;

    public InputActionProperty retryButton;

    bool triggerChallenge=false;
    bool startGame=false;
    bool nearEndTimer=false;
    bool autoBuyed=false;
    bool failed=false;
    int currentMoney=0;
    float currentTimer;
    // Start is called before the first frame update
    void Start()
    {
        currentTimer = gameoverTimer;
    }
    // Update is called once per frame
    void Update()
    {
        // prepare start game
        if (startGame == false)
        {
            if (startTimer > 0)
                startTimer-= Time.deltaTime;
            else
            {
                startCanvas.gameObject.SetActive(true);
                startGame=true;
            }   
        }
        // calculate and display timer when manual pumping
        if (currentTimer > 0 && triggerChallenge == true && startGame==true)
        {
            currentTimer -= Time.deltaTime;
            timeText.text = Mathf.RoundToInt(currentTimer).ToString() + "s";
            if (currentTimer < 10)
                timeText.color = Color.red;
            if (currentTimer <= 3 && nearEndTimer == false)
            {
                audioSource.PlayOneShot(over);
                nearEndTimer = true;
            }      
        }
        // update farm field
        if ((pumpbar.waterBar.fillAmount >= 0.5f && riceField.currentRiceField == 0) || (pumpbar.waterBar.fillAmount >= 1.0f && riceField.currentRiceField == 1))
            riceField.GrowFarm();

        // check win or lose game
        if (failed == false)
        {
            if (currentTimer <= 0)
            {
                LoseGame();
                failed = true;
            }
            else
            {
                if (pumpbar.waterBar.fillAmount == 1)
                    if (firstWaterTimes==0)
                        if (secondWaterTimes == -1)
                            TriggerLandSubside();
                        else
                            EnableBuyAutoSystem();
                    else
                        WinGame();
            }
        }
        // check after player bought auto system
        if (autoBuyed == true)
        {
            if (pumpbar.waterBar.fillAmount==1)
                if (secondWaterTimes==0)
                    TriggerLandSubside();
                else
                    WinGameWithAuto();
            else
            {
                if (currentTimer <= 0)
                {
                    pumpbar.UpdateAutoWaterSystem();
                    currentTimer=1;
                }  
                else
                    currentTimer-=Time.deltaTime;  
                
                if (pumpbar.waterBar.fillAmount == 0.2f)
                    riceField.ResetFarm();
            }
      
        }
        //restartgame
        if (retryButton.action.WasPressedThisFrame() &&  ( gameOverText.gameObject.activeSelf || landSubsideText.gameObject.activeSelf))
        {
            RetryGame();
        }
        
    }

    public void ChooseWaterTheField(bool yes)
    {
        audioSource.PlayOneShot(select);
        startCanvas.gameObject.SetActive(false);
        triggerChallenge = true;
        if (yes==true)
        {
            helperCanvas.gameObject.SetActive(true);
            timerAudioSource.gameObject.SetActive(true);
            transform.position = teleportPoint.position;
            firstWaterTimes--;
        }
        else
            currentTimer = gameoverTimer/2f;
    }

    public void WinGame()
    {
        UpdateUIWhenWin();
        pumpbar.enabled = false;
        pumpbar.waterBar.fillAmount = 0;
        triggerChallenge=false;
    }

    public void NextGame()
    {
        UpdateUIWhenNext();
        pumpbar.ResetWaterPosition();
        riceField.ResetFarm();
        pumpbar.enabled = true;
        triggerChallenge=true;
        firstWaterTimes--;
        currentTimer = gameoverTimer;

        
    }

    public void WinGameWithAuto()
    {
        UpdateUIWhenWin();
        pumpbar.ResetWaterPosition();
        pumpbar.waterBar.fillAmount = 0;
        secondWaterTimes--;
    }

    public void LoseGame()
    {
        UpdateUIWhenLose();
        riceField.DeadFarm();
        gameOverText.gameObject.SetActive(true);
        pumpbar.enabled = false;
        pumpbar.waterBar.fillAmount = 0;
        triggerChallenge=false;
        currentTimer = gameoverTimer;
    }

    public void RetryGame()
    {
        riceField.ReviveFarm(); 
        SceneManager.LoadScene("VR Scene");
    }

    public void EnableBuyAutoSystem()
    {
        UpdateUIWhenWin();
        helperCanvas.gameObject.SetActive(false);
        buycanvas.gameObject.SetActive(true);
        pumpbar.enabled = false;
        pumpbar.waterBar.fillAmount = 0;
        triggerChallenge = false;
    }

    public void BuyAutoSystem()
    {
        helperCanvas.gameObject.SetActive(false);
        manualHandPump.gameObject.SetActive(false);
        autoHandPumps.gameObject.SetActive(true);
        pumpbar.ResetWaterPosition();
        riceField.ResetFarm();
        currentMoney -= autoSystemCost;
        moneyText.text = currentMoney.ToString("N0");
        autoBuyed = true;
        failed = true;
        firstWaterTimes = -1;
        currentTimer = 1;
    }

    public void NotBuyAutoSystem()
    {
        UpdateUIWhenNext(); 
        helperCanvas.gameObject.SetActive(true);
        buycanvas.gameObject.SetActive(false);
        pumpbar.ResetWaterPosition();
        pumpbar.enabled = true;
        triggerChallenge = true;
        autoBuyed = false;
        firstWaterTimes = secondWaterTimes;
        secondWaterTimes=-1;
        currentTimer = gameoverTimer;
    }

    public void TriggerLandSubside()
    {
        UpdateUIWhenLose();
        landAudioSource.gameObject.SetActive(true);
        landSubsideText.gameObject.SetActive(true);
        pumpbar.enabled = false;
        pumpbar.waterBar.fillAmount = 0;
        triggerChallenge=false;
        autoBuyed = false;
        failed = true;
        secondWaterTimes=-1;
        currentTimer = gameoverTimer;
        var rigidbodies = mudField.gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.useGravity = true;
        }
        riceField.RemoveFarm();
        pumpbar.water.gameObject.GetComponent<Animator>().enabled = true;
        
    }

    // UTILITIES
    void UpdateUIWhenWin()
    {
        audioSource.PlayOneShot(reward);
        floatingText.text = "+" + moneyAmount.ToString("N0");
        floatingText.gameObject.SetActive(false);
        floatingText.gameObject.SetActive(true);
        currentMoney+=moneyAmount;
        moneyText.text = currentMoney.ToString("N0");

        timerAudioSource.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        infoText.gameObject.SetActive(false);
        if (autoBuyed==false)
        {
            readyButton.gameObject.SetActive(true);
            readyText.gameObject.SetActive(true);
        }
    }

    void UpdateUIWhenLose()
    {
        audioSource.PlayOneShot(lose);
        timerAudioSource.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
    }

    void UpdateUIWhenNext()
    {
        timerAudioSource.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);
        timeText.text = Mathf.RoundToInt(gameoverTimer).ToString() + "s";
        readyButton.gameObject.SetActive(false);
        readyText.gameObject.SetActive(false);
    }

}
