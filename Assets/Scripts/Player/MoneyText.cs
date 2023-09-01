using UnityEngine;
using TMPro;

public class MoneyText : MonoBehaviour
{
    public PlayerStat _playerStat;
    public TextMeshProUGUI moneyTxt;

    private void Update()
    {
        UpdateMoneyTxt();
    }

    /// <summary>
    /// Updates the money text to display the current amount of money the player has.
    /// </summary>
    private void UpdateMoneyTxt()
    {
        moneyTxt.text = _playerStat.CurrentMoney.ToString();
    }
}
