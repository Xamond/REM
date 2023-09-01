using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField]
    private float startingTime = 60;
    private float currentTime;
    private float savedTime;
    private bool isPaused = false;
    private float timeBonusMultiplicator;
    private float bonusTimeValue = 1f;

    private TextMeshProUGUI countdownText;
    private TextMeshProUGUI bonusTimeText;

    private void Awake()
    {
        SetTimerPanelTxtElements();
    }

    private void Start()
    {
        currentTime = startingTime;
    }

    private void Update()
    {
        if (!isPaused)
            StartCountdown();

        ChangeTextColor(currentTime);
    }

    ///<summary>
    ///Finds and sets the TextMeshProUGUI elements for the countdown timer and bonus time display.
    ///</summary>
    private void SetTimerPanelTxtElements()
    {
        GameObject timerPanel = GameObject.Find("TimerPanel")?.gameObject;
        GameObject countdownTxtObj = timerPanel.transform.GetChild(0).gameObject;
        GameObject bonusTimeTxtObj = timerPanel.transform.GetChild(1).gameObject;

        countdownText = countdownTxtObj.GetComponent<TextMeshProUGUI>();
        bonusTimeText = bonusTimeTxtObj.GetComponent<TextMeshProUGUI>();

        bonusTimeTxtObj.SetActive(false);
    }

    public void StartCountdown()
    {
        isPaused = false;
        if(currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = ((int)currentTime).ToString();
        }
    }

    ///<summary>
    ///Changes the color of the countdown timer text based on the 
    ///current time remaining in the countdown.
    ///</summary>
    private void ChangeTextColor(float currentTime)
    {
        if(currentTime <= 30 && currentTime > 10)
            countdownText.color = new Color32(255,116,0,255);
        else if(currentTime <= 10)
            countdownText.color = Color.red;
        else if(currentTime > 30)
            countdownText.color = Color.white;
    } 


    public void PauseCountdown()
    {
        isPaused = true;
        savedTime = currentTime;
        currentTime = savedTime;
        countdownText.text = ((int)currentTime).ToString();
    }

    public void ResumeCountdown()
    {
        isPaused = false;
    }

    /// <summary>
    /// Adds bonus time to the countdown timer and updates the bonus time text.
    /// </summary>
    /// <param name="timeInSec">The amount of bonus time to add in seconds.</param>
    public void AddBonusTime(float timeInSec)
    {
        AdjustTimeBonus();
        bonusTimeValue = timeInSec * timeBonusMultiplicator;
        currentTime += bonusTimeValue;
        bonusTimeText.text = "+ " + ((int)bonusTimeValue).ToString();

        if(bonusTimeValue > 0)
            StartCoroutine(DisplayBonusTime(0.7f));
    }

    /// <summary>
    /// Adjusts the time bonus multiplier based on the player's current combo rank.
    /// The higher the rank, the higher the multiplier. The default multiplier is 1f.
    /// </summary>
    private void AdjustTimeBonus()
    {
        PlayerStat _playerStat = GameObject.FindObjectOfType<PlayerStat>();

        switch (_playerStat.CurrentComboRank)
        {
            case "C":
                timeBonusMultiplicator = 1f;
                break;
            case "B":
                timeBonusMultiplicator = 2f;
                break;
            case "A":
                timeBonusMultiplicator = 3f;
                break;
            case "S":
                timeBonusMultiplicator = 5f;
                break;
        }
    }
    IEnumerator DisplayBonusTime(float displayDuration)
    {
        AudioManager _audioManager = FindObjectOfType<AudioManager>();
        _audioManager.PlaySFX(_audioManager.TimeBonusSFX);

        bonusTimeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        bonusTimeText.gameObject.SetActive(false);
    }

    //Getter & Setter
    public float CurrentTime 
    { 
        get{return currentTime;}
        set{currentTime = value;}
    }
}
