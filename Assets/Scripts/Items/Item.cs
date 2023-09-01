using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    [SerializeField]
    protected int price;
    [SerializeField]
    protected AudioClip sfx;
    [SerializeField]
    private Image icon;
    [SerializeField]
    protected List<GameObject> cardObjsOnField;


    private CardManager _cardManager;
    protected AudioSource _audioSource;
    protected bool canUse;

    private void Awake()
    {
        _audioSource = GameObject.Find("ItemManager").GetComponent<AudioSource>();
        _cardManager = FindObjectOfType<CardManager>();
    }

    private void Start()
    {
        canUse = true;
    }

    private void Update()
    {
        cardObjsOnField = _cardManager.CardObjsOnField;
    }

    /// <summary>
    /// Subtracts the given amount from the player's current money.
    /// </summary>
    /// <param name="amount">The amount to subtract from the player's current money.</param>
    protected void SubtractMoney(int amount)
    {
        PlayerStat _playerStat = FindObjectOfType<PlayerStat>();
        _playerStat.CurrentMoney -= amount;
    }

    public Image Icon {get{return icon;}}
    public int Price 
    {
        get{return price;}
        set{price = value;}
    }
}
