using UnityEngine;
using TMPro;

public class ComboRankTxt : MonoBehaviour
{
    public PlayerStat _playerStat;
    public TextMeshProUGUI comboRankText;

    private void Start()
    {
        AdjustComboRankText();
    }

    private void Update()
    {
        UpdateComboRankTxt();
    }

    /// <summary>
    /// Updates the combo rank text to display the current combo rank from the _playerStat field.
    /// </summary>
    private void UpdateComboRankTxt()
    {
        comboRankText.text = _playerStat.CurrentComboRank.ToString();
    }

    /// <summary>
    /// Adjusts the color of the combo rank text based on the player's current combo rank.
    /// </summary>
    public void AdjustComboRankText()
    {
        switch(_playerStat.CurrentComboRank)
        {
            case "D":
                comboRankText.color = Color.green;
                break;
            case "C":
                comboRankText.color = Color.cyan;
                break;
            case "B":
                comboRankText.color = Color.magenta;
                break;
            case "A":
                comboRankText.color = Color.red;
                break;
            case "S":
                comboRankText.color = Color.yellow;
                break;
        }
    }
}
