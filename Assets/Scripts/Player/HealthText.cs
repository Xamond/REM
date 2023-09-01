using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    [SerializeField]
    private PlayerStat playerHealth;
    [SerializeField]
    private TextMeshProUGUI textHealth;

    private void Update()
    {
        AdjustPlayerHealthTxt();
        AdjustHealthTxtColor();
    }

    /// <summary>
    /// Updates the player's health text based on their current health.
    /// </summary>
    private void AdjustPlayerHealthTxt()
    {
        if(playerHealth.CurrentHealth > 0)
            textHealth.text = "HP: " + playerHealth.CurrentHealth.ToString();
        else
            textHealth.text = "HP: 0";
    }

    /// <summary>
    /// Adjusts the color of the health text based on the player's current health.
    /// </summary>
    private void AdjustHealthTxtColor()
    {
        if(playerHealth.CurrentHealth <= 50 && playerHealth.CurrentHealth > 30)
            textHealth.color = Color.yellow;
        else if(playerHealth.CurrentHealth <= 30 && playerHealth.CurrentHealth > 10)
            textHealth.color = new Color32(255,116,0,255);
        else if(playerHealth.CurrentHealth <= 10)
            textHealth.color = Color.red;
        else
            textHealth.color = Color.white;
    }
}
