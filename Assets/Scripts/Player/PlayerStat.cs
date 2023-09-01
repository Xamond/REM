using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    private float currentHealth = 100f;
    private int currentMoney = 0;
    [SerializeField]
    private AudioSource playerAudioSource;
    [SerializeField]
    private int moneyValue;
    
    private int comboCounter;
    private int currencyMultiplicator;
    private string currentComboRank;

    void Start()
    {
      comboCounter = 0;
      currencyMultiplicator = 1;
      currentComboRank = "D";
      currentMoney = moneyValue;
    }

    void Update()
    {
      ControlDangerSFX();
      AdjustHealthStatFX();
    }

    /// <summary>
    /// Adds money to the player's current money, with an optional multiplier.
    /// </summary>
    /// <param name="multiplicator">The multiplier to apply to the money value (default is 1).</param>
    public void AddMoneyToPlayer(int multiplicator)
    {
        if(currentMoney < 99)
            currentMoney += 2 * multiplicator;
    }

    /// <summary>
    /// Increases the player's combo counter by 1.
    /// </summary>
    public void RaiseComboStat()
    {
      comboCounter += 1;
    }

    /// <summary>
    /// Resets the player's combo counter to 0.
    /// </summary>
    public void ResetComboStat()
    {
      comboCounter = 0;
    }

    /// <summary>
    /// Raises the player's combo rank based on their current combo rank.
    /// </summary>
    /// <param name="_comboRankTxt">The combo rank text object to adjust.</param>
    public void RaiseComboRank(ComboRankTxt _comboRankTxt)
    {
      switch(currentComboRank)
      {
        case "D":
          currentComboRank = "C";
          break;
        case "C":
          currentComboRank = "B";
          break;
        case "B":
          currentComboRank = "A";
          break;
        case "A":
          currentComboRank = "S";
          break;
      }

      _comboRankTxt.AdjustComboRankText();
    }

    /// <summary>
    /// Resets the player's combo rank to D and hides the combo rank UI element.
    /// </summary>
    /// <param name="_comboRankStat">The combo rank UI element to hide.</param>
    /// <param name="_comboRankText">The combo rank text object to adjust.</param>
    public void ResetComboRank(GameObject _comboRankStat, ComboRankTxt _comboRankText)
    {
      _comboRankStat.SetActive(false);
      currentComboRank = "D";
      _comboRankText.AdjustComboRankText();
    }

    /// <summary>
    /// Adjusts the player's currency multiplier based on their current combo counter and combo rank.
    /// </summary>
    /// <param name="comboCounter">The player's current combo counter.</param>
    public void AdjustComboBonus(int comboCounter)
    {
      GameObject _comboRankPanel = GameObject.Find("ComboRank_Panel");
      GameObject _comboRankStat = _comboRankPanel.transform.GetChild(0).gameObject;
      ComboRankTxt _comboRankTxt = FindObjectOfType<ComboRankTxt>();

      AdjustComboRank(_comboRankPanel, _comboRankStat, _comboRankTxt);
    }

    /// <summary>
    /// Adjusts the combo rank based on the current combo counter.
    /// </summary>
    /// <param name="_comboRankPanel">The panel that displays the combo rank.</param>
    /// <param name="_comboRankStat">The game object that displays the combo rank stat.</param>
    /// <param name="_comboRankTxt">The combo rank text component.</param>
    private void AdjustComboRank(GameObject _comboRankPanel, GameObject _comboRankStat, ComboRankTxt _comboRankTxt)
    {
        if (comboCounter == 1)
        {
            _comboRankStat.SetActive(true);
            RaiseComboRank(_comboRankTxt);
        }
        else if (comboCounter == 2)
        {
            currencyMultiplicator = 2;
            RaiseComboRank(_comboRankTxt);
        }
        else if (comboCounter >= 3 && comboCounter < 5)
        {
            currencyMultiplicator = 3;
            RaiseComboRank(_comboRankTxt);
        }
        else if (comboCounter >= 5)
        {
            currencyMultiplicator = 5;
            RaiseComboRank(_comboRankTxt);
        }
        else
        {
            currencyMultiplicator = 1;
            ResetComboRank(_comboRankStat, _comboRankTxt);
        }
    }

    /// <summary>
    /// Adjusts the opacity of the health status effect based on the current health of the player.
    /// </summary>
    public void AdjustHealthStatFX()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject healthStatFx = canvas.transform.GetChild(0).gameObject;
        Image healthStatFxImg = healthStatFx.GetComponent<Image>();
        float opacityVar = (50f - currentHealth) * 0.01f;

        if(currentHealth <= 50f)
        {
            SetOpacity(healthStatFxImg, opacityVar); 
        }
        else
        {
            SetOpacity(healthStatFxImg, 0f);
        }
    }

    /// <summary>
    /// Controls the danger sound effect depending on the player's current health.
    /// </summary>
    private void ControlDangerSFX()
    {
        if(currentHealth <= 30 && !playerAudioSource.isPlaying)
        {
          playerAudioSource.Play();
        }
        else if(currentHealth > 30 || currentHealth <= 0)
        {
          playerAudioSource.Stop();
        }
    }

    /// <summary>
    /// Sets the opacity of the given image using the specified opacity variable.
    /// </summary>
    /// <param name="image">The image to modify the opacity of.</param>
    /// <param name="opacityValue">The opacity value to set (between 0 and 1).</param>
    public void SetOpacity(Image image, float opacityValue)
    {
        Color tmp = image.color;
        tmp.a = opacityValue;
        image.color = tmp;
    }

    //Getter & Setter
    public float CurrentHealth
    {
        get {return currentHealth;}
        set {currentHealth = value;}
    }

    public int CurrentMoney
    {
        get {return currentMoney;}
        set {currentMoney = value;}
    }

    public int ComboCounter
    {
        get {return comboCounter;}
        set {comboCounter = value;}
    }
    public int CurrencyMultiplicator
    {
        get{return currencyMultiplicator;}
        set{currencyMultiplicator = value;}
    }
    
    public string CurrentComboRank
    {
        get{return currentComboRank;}
        set{currentComboRank = value;}
    }

}
