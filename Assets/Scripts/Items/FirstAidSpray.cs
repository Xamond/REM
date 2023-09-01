using System.Collections;
using UnityEngine;

public class FirstAidSpray : Item
{
    private float healAmount;

    /// <summary>
    /// Heals the player by a certain amount if the player has enough money and the item can be used.
    /// </summary>
    /// <param name="price">The price of the item.</param>
    public void UseFirstAidSpray(int price)
    {
        PlayerStat _playerStat = FindObjectOfType<PlayerStat>();

        if(_playerStat.CurrentMoney >= price && canUse)
        {
            base.SubtractMoney(price);
            base._audioSource.PlayOneShot(sfx);
            StartCoroutine(DelayedHeal(0.5f));
        }
    }

    /// <summary>
    /// Adjusts the amount of healing based on the player's current health.
    /// </summary>
    /// <param name="currentPlayerHP">The player's current health.</param>
    /// <returns>The adjusted amount of healing.</returns>
    private float AdjustHealAmount(float currentPlayerHP)
    {
        if(currentPlayerHP >= 30f)
            return 100f;
        else
            return (currentPlayerHP + 70f);
    }

    /// <summary>
    /// Delays the healing process by a certain amount of time and then heals the player.
    /// </summary>
    /// <param name="delayTime">The amount of time to delay the healing process.</param>
    /// <returns>An enumerator for the coroutine.</returns>
    IEnumerator DelayedHeal(float delayTime)
    {
        PlayerStat _playerStat = FindObjectOfType<PlayerStat>();

        canUse = false;
        yield return new WaitForSeconds(delayTime);
        healAmount = AdjustHealAmount(_playerStat.CurrentHealth);
        _playerStat.CurrentHealth = healAmount;
        canUse = true;
    }  
}
