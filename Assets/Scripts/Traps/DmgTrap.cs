using System.Collections;
using UnityEngine;
using TMPro;

public class DmgTrap : Trap
{
    private PlayerStat _playerStat;
    private int damageValue;
    private DmgTrapData data;

    [SerializeField]
    private TextMeshProUGUI healthStatDmgFxText;

    void Awake()
    {
        _playerStat = FindObjectOfType<PlayerStat>();
    }

    void Start()
    {
        LoadTrapData();
        InitializeTrapProperties();
    }

    // <summary>
    // Loads the trap data for this specific trap type from a ScriptableObject and assigns it to the 'data' variable.
    // </summary>
    // <param name="path">The path of the ScriptableObject file to load.</param>
    private void LoadTrapData()
    {
        string path = "TrapDataSO/" + type.ToString();
        data = Resources.Load<DmgTrapData>(path);
    }


    // <summary>
    // Initializes the trap's properties, setting its damage value
    // to the value specified in the loaded trap data.
    // </summary>
    private void InitializeTrapProperties()
    {
        damageValue = data.Damage;
    }

    public override void ActivateTrap()
    {
        SetHealthStatDmgFXText(damageValue);
        DamagePlayer(damageValue);
    }

    /// <summary>
    /// Sets the text of the health stat FX to display the given damage value.
    /// </summary>
    /// <param name="damageValue">The amount of damage to display.</param>
    private void SetHealthStatDmgFXText(int damageValue)
    {
        healthStatDmgFxText.text = "- " + damageValue.ToString() + "HP";
    }

    /// <summary>
    /// Starts the coroutine to show the health stat change effect and inflict damage to the player
    /// </summary>
    /// <param name="dmgValue">The amount of damage to be inflicted to the player</param>
    private void DamagePlayer(int dmgValue)
    {
        StartCoroutine(FlashHealthStatChangeAndDamagePlayer(dmgValue, 0.5f));
    }

    /// <summary>
    /// Coroutine that shows the health stat change effect, plays a trap sound effect,
    /// inflicts damage to the player and hides the health stat change effect.
    /// </summary>
    /// <param name="dmgValue">The amount of damage to be inflicted to the player</param>
    /// <param name="trapSfxVolume">The volume of the trap sound effect</param>
    IEnumerator FlashHealthStatChangeAndDamagePlayer(int dmgValue, float trapSfxVolume)
    {
        AudioManager _audioManager = FindObjectOfType<AudioManager>();

        if(type == TrapType.BEARTRAP)
        {
            _audioManager.PlaySFX(_audioManager.ClipDmgTraps[0], trapSfxVolume);
        }
        else
        {
            _audioManager.PlaySFX(_audioManager.ClipDmgTraps[1], trapSfxVolume);
            yield return new WaitForSeconds(2.243f);
        }

        healthStatDmgFxText.gameObject.SetActive(true);
        _playerStat.CurrentHealth -= dmgValue;
        yield return new WaitForSeconds(0.7f);
        healthStatDmgFxText.gameObject.SetActive(false);
    }
}


