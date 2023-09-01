using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;

public class TimeTrap : Trap
{
    private CountdownTimer _timerInst;
    [SerializeField]
    private TextMeshProUGUI bonusTimeText;

    private void Awake()
    {
        _timerInst = FindObjectOfType<CountdownTimer>();
        type = TrapType.TIMETRAP;
    }

    public override void ActivateTrap()
    {
        SubtractTime(5);
    }

    /// <summary>
    /// Subtracts the specified amount of time from the
    /// countdown timer and displays the appropriate visual effect.
    /// </summary>
    /// <param name="timeInSeconds">The amount of time to subtract, in seconds.</param>
    private void SubtractTime(int timeInSeconds)
    {
        _timerInst.CurrentTime -= timeInSeconds;
        bonusTimeText.text = "- " + timeInSeconds;

        StartCoroutine(FlashTimeFX(0.7f, 0.5f));
    }

    /// <summary>
    /// Flashes the bonus time text, plays a sound effect
    /// and changes the color of the text to red while active.
    /// </summary>
    /// <param name="delayTime">The duration of time the text is displayed.</param>
    /// <param name="sfxVolume">The volume of the sound effect played.</param>
    IEnumerator FlashTimeFX(float delayTime, float sfxVolume)
    {
        AudioManager _audioManager = FindObjectOfType<AudioManager>();
        _audioManager.PlaySFX(_audioManager.TimeDownSFX, sfxVolume);
        bonusTimeText.color = Color.red;
        bonusTimeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delayTime);
        bonusTimeText.gameObject.SetActive(false);
        bonusTimeText.color = Color.green;
    }
}
