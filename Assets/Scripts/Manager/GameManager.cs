using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private CardManager _cardManager;
    private AudioManager _audioManager;
    private PlayerStat _playerStat;
    private CountdownTimer _timer;
    private List<StageData> _stageDataList = new List<StageData>();
    private int stageIndex = 0;

    [SerializeField]
    private Card[] selectedCards = new Card[2];

    private bool canClick = true;


    [SerializeField]
    private Enemy currentEnemy;
    [SerializeField]
    private int dmgMultiplicator;

    private List<Vector3> allPositions = new List<Vector3>();
    
    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        _playerStat = FindObjectOfType<PlayerStat>();
        _timer = FindObjectOfType<CountdownTimer>();
        InitializeStageDataList("StagesSO");
        InitializeStageData(stageIndex);
    }

    private void Start()
    {
        _cardManager.SaveCardObjPositions(_cardManager.CardObjsOnField, allPositions);
        _cardManager.ShuffleCards(_cardManager.CardObjsOnField, allPositions);
    }

    private void Update()
    {
        _cardManager.AnyCardsLeft = _cardManager.CardObjsOnField.Any();
        HandleEndGameConditions(_playerStat, _cardManager.AnyCardsLeft);
        RemoveNullCardsFromField(_cardManager.AnyCardsLeft);

    }

    private void InitializeStageDataList(string folder)
    {
        _stageDataList = Resources.LoadAll<StageData>(folder).ToList();
    }

    private void InitializeStageData(int stageIndex)
    {
        StageData _stageData = _stageDataList[stageIndex];

        _cardManager.CardDataPath = _stageData.CardDataFolderPath;
        _cardManager.Spacing = _stageData.Spacing;
        _cardManager.NumRows = _stageData.NumRows;
        _cardManager.NumCols = _stageData.NumCols;
        _cardManager.MarginX = _stageData.MarginX;
        _cardManager.MarginY = _stageData.MarginY;
    }

    /// <summary>
    /// This method removes any null cards from the list of card objects on the field.
    /// </summary>
    /// <param name="hasCards">A boolean value indicating if there are any cards on the field.</param>
    public void RemoveNullCardsFromField(bool hasCards)
    {
        if(hasCards)
            _cardManager.CardObjsOnField = _cardManager.CardObjsOnField.Where(cardObj => cardObj != null).ToList();
    }

    /// <summary>
    /// Handles the logic when a card is clicked by the player.
    /// </summary>
    /// <param name="card">The card object that was clicked.</param>
    public void OnCardClicked(Card card)
    {
        if(canClick == false || card == selectedCards[0])    
            return;

        _cardManager.FlipCard(card);
        _audioManager.PlaySFX(_audioManager.CardSFXClips[1], 0.2f);

        if(selectedCards[0] == null)
        {
            selectedCards[0] = card;
        }
        else
        {
            selectedCards[1] = card;
            canClick = false;
            Invoke("CheckMatch", 1);
        }
    }

    /// <summary>
    /// Checks if the two selected cards are a match and triggers the corresponding events.
    /// </summary>
    /// <param name="selectedCards">An array containing the two selected cards.</param>
    public void CheckMatch()
    {
        bool matchingID = selectedCards[0].Identifier == selectedCards[1].Identifier;
        bool isTrap = selectedCards[0].IsTrap && selectedCards[1].IsTrap;

        if (matchingID && !isTrap || matchingID && !HasNormalCard())
        {
            OnMatch();
        }
        else if (matchingID && isTrap && HasNormalCard())
        {
            OnTrapActivation();
        }
        else
        {
            OnMismatch();
        }

        ResetSelection();
    }

    /// <summary>
    /// Check if there is at least one normal card (not a trap) left on the game field.
    /// </summary>
    /// <returns>Returns true if there is at least one normal card left, false otherwise.</returns>
    private bool HasNormalCard()
    {
        for(int i = 0; i < _cardManager.CardObjsOnField.Count; i++)
        {
            if(_cardManager.CardObjsOnField[i].GetComponent<Card>().IsTrap == false)
                return true;
        }
        return false;
    }

    private void RemoveSelectedCards(Card[] selectedCards)
    {
        for(int i = 0; i < selectedCards.Length; i++)
        {
            _cardManager.CardObjsOnField.Remove(selectedCards[i].gameObject);
            Destroy(selectedCards[i].gameObject);
        }
    }

    private void DamagePlayer(float multiplicator)
    {
        PlayerStat _playerStat = FindObjectOfType<PlayerStat>();        
        float dmg = Random.Range(1, 10) * multiplicator;
        _playerStat.CurrentHealth = _playerStat.CurrentHealth > 0 ? _playerStat.CurrentHealth - dmg : 0;
    }

    /// <summary>
    /// Checks if the end game conditions have been met and displays the appropriate screen if necessary.
    /// </summary>
    /// <param name="_playerStat">The player's statistics object.</param>
    /// <param name="hasCards">Whether or not there are still cards on the game field.</param>
    private void HandleEndGameConditions(PlayerStat _playerStat, bool hasCards)
    {
        var screenData = GetScreenData();
        GameObject deathScreen = screenData.deathScreen;
        GameObject winScreen = screenData.winScreen;

        if((_playerStat.CurrentHealth <= 0 || _timer.CurrentTime <= 0) && !deathScreen.activeSelf)
        {
            DisplayDeathScreen();   
        }
        else if((!hasCards && SceneManager.GetActiveScene().buildIndex != 0) && !winScreen.activeSelf)
        {
            DisplayWinScreen();
        }
    }


    /// <summary>
    /// Retrieves the timer, inventory panel, win screen, death screen 
    /// gameobjects and returns them.
    /// </summary>
    /// <returns>A tuple containing the timer, inventory panel, win screen, and death screen.</returns>
    private (CountdownTimer timer, GameObject invPanel, GameObject winScreen, GameObject deathScreen) 
    GetScreenData()
    {
        CountdownTimer timer = FindObjectOfType<CountdownTimer>();
        GameObject canvas = GameObject.Find("Canvas");
        GameObject winScreen = canvas.transform.Find("WinScreen").gameObject;
        GameObject deathScreen = canvas.transform.Find("DeathScreen").gameObject;
        GameObject invCanvas = GameObject.Find("InventoryCanvas");
        GameObject invPanel = invCanvas.transform.Find("InventoryPanel").gameObject;

        return (timer, invPanel, winScreen, deathScreen);
    }

    private void DisplayDeathScreen()
    {
        var (timer, invPanel, winScreen, deathScreen) = GetScreenData();
        
        
        if (deathScreen.activeSelf || winScreen.activeSelf)
            return;

        canClick = false;
        timer.CurrentTime = 0;
        ToggleInventory(invPanel);
        deathScreen.SetActive(true);
        PlayDefeatSFX();
    }

    private void DisplayWinScreen()
    {
        var (timer, invPanel, winScreen, deathScreen) = GetScreenData();

        if (winScreen.activeSelf || deathScreen.activeSelf)
            return;

        canClick = false;
        timer.PauseCountdown();
        ToggleInventory(invPanel);
        winScreen.SetActive(true);
        ToggleOptionPanel();
        PlayOutroSFX();
    }

    private void ToggleInventory(GameObject invPanel)
    {
        if (invPanel.activeSelf == true)
            invPanel.SetActive(false);
        else
            invPanel.SetActive(true);
    }

    /// <summary>
    /// Toggles the option panel based on the current stage index and the win screen gameobject.
    /// </summary>
    private void ToggleOptionPanel()
    {
        var stageData = GetScreenData();
        GameObject winScreen = stageData.winScreen;

        string optionPanelName;

        DeactivateAllOptionPanels(winScreen);
        optionPanelName = GetOptionPanelName(stageIndex);
        ActivateOptionPanel(winScreen, optionPanelName);
    }


    private void DeactivateAllOptionPanels(GameObject winScreen)
    {
        foreach (Transform child in winScreen.transform)
        {
            if (child.name.Contains("OptionPanel"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void ActivateOptionPanel(GameObject winScreen, string panelName)
    {
        GameObject optionPanel = winScreen.transform.Find(panelName).gameObject;
        optionPanel.SetActive(true);
    }

    private string GetOptionPanelName(int stageIndex)
    {
        return stageIndex >= _stageDataList.Count -1 ? "OptionPanelFin" : "OptionPanel";
    }

    private void PlayOutroSFX()
    {
        GameObject bgm = GameObject.Find("BGM");
        bgm.GetComponent<AudioSource>().Stop();
        _audioManager.AudioSourceInst.Stop();
        _audioManager.PlaySFX(_audioManager.EnemyOutroVoiceline[stageIndex], 1.2f);
    }

    private void PlayDefeatSFX()
    {
        List<AudioClip> enemyWinsVoiceSFX = _audioManager.EnemyWinsVoicelines;
        GameObject bgm = GameObject.Find("BGM");
        bgm.GetComponent<AudioSource>().Stop();
        int randomIndex = Random.Range(0, enemyWinsVoiceSFX.Count);
        _audioManager.PlaySFX(enemyWinsVoiceSFX[randomIndex], 1.2f);
        _audioManager.PlaySFX(_audioManager.DefeatSFX, 0.5f);   
    }

    /// <summary>
    /// Executes the necessary actions when a trap is activated.
    /// </summary>
    private void OnTrapActivation()
    {
        currentEnemy.ActivateTrap(selectedCards[0].AttachedTrap);
        _cardManager.FlipCards(selectedCards);
        _audioManager.PlaySFX(_audioManager.ClipAccessDenied);
    }

    /// <summary>
    /// Executes the necessary actions when two cards are matched.
    /// </summary>
    private void OnMatch()
    {
        RemoveSelectedCards(selectedCards);
        _playerStat.AddMoneyToPlayer(_playerStat.CurrencyMultiplicator);
        _playerStat.RaiseComboStat();
        _playerStat.AdjustComboBonus(_playerStat.ComboCounter);
        _timer.AddBonusTime(1f);
        _audioManager.PlaySFX(_audioManager.ClipMatchCard);
    }

    /// <summary>
    /// Handles the logic when the two selected cards do not match. 
    /// Damages the player, adjusts the combo bonus and plays a sound effect.
    /// </summary>
    private void OnMismatch()
    {
        DamagePlayer(dmgMultiplicator);
        _cardManager.FlipCards(selectedCards);
        _playerStat.ResetComboStat();
        _playerStat.AdjustComboBonus(_playerStat.ComboCounter);
        _audioManager.PlaySFX(_audioManager.CardSFXClips[0], 0.2f);
    }

    private void ResetSelection()
    {
        selectedCards[0] = null;
        selectedCards[1] = null;
        canClick = true;
    }

    public void LoadNextStage()
    {
        // Retrieve screen data
        var screenData = GetScreenData();

        // Find and play background music game object
        GameObject bgm = GameObject.Find("BGM");
        bgm.GetComponent<AudioSource>().Play();

        // Increment next stage index
        stageIndex++;

        // Get next stage data and refresh cards
        StageData nextStageData = _stageDataList[stageIndex];
        _cardManager.RefreshCards(nextStageData);

        // Set new traps
        currentEnemy.SetMultipleTraps();

        // Resume timer and add bonus time
        _timer.ResumeCountdown();
        _timer.AddBonusTime(10f);

        // Toggle inventory switch panel
        ToggleInventory(screenData.invPanel);

        // Enable clicking
        canClick = true;
    }

    //Getter & Setter
    public bool CanClick
    {
        get{return canClick;}
        set{canClick = value;}
    } 
}
