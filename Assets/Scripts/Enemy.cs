using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private new string name;
    [SerializeField]
    private CardManager _cardManager;
    [SerializeField]
    private List<Trap> _traps;
    
    private AudioManager _audioManager;
    private List<GameObject> _cardObjsOnField = new List<GameObject>();

    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _cardObjsOnField = FindObjectOfType<CardManager>().CardObjsOnField;
    }

    private void Start()
    {
        SetMultipleTraps();
    }

    private void Update()
    {
        _cardObjsOnField = _cardManager.CardObjsOnField;
    }

    public void SetMultipleTraps()
    {
        for(int i = 0; i < _traps.Count; i++)
        {
            SetTrap(i);
        }
    }
    private void SetTrap(int index)
    {
        if (_cardManager.CardObjsOnField.Count == 0)
            return;

        Card trapCard = _cardManager.CardObjsOnField[index].GetComponent<Card>();
        TurnCardIntoTrap(trapCard, index);
        Card identicalCard = FindIdenticalCard(trapCard);
        TurnCardIntoTrap(identicalCard, index);
    }

    private void TurnCardIntoTrap(Card inpCard, int trapsIndex)
    {
        inpCard.IsTrap = true;
        inpCard.AttachedTrap = _traps[trapsIndex];
    }

    private Card FindIdenticalCard(Card inpCard)
    {
        GameObject cardObject;
        Card cardToCompare;

        for(int i = 0; i < _cardManager.CardObjsOnField.Count; i++)
        {
            cardObject = _cardManager.CardObjsOnField[i];
            cardToCompare = cardObject.GetComponent<Card>();

            if(cardToCompare.Identifier == inpCard.Identifier && !cardToCompare.IsTrap)
            {
                return cardToCompare;
            }
        }
        return null;
    }

    public void ActivateTrap(Trap inpTrap)
    {
        Trap selectedTrap = null;
        selectedTrap = _traps.Find(trap => trap.Type == inpTrap.Type);

        if (_traps.Count > 0 && selectedTrap != null) 
        {
            _audioManager.PlayRndTrapVoicelineSFX(1.5f);
            selectedTrap.ActivateTrap();
        }
    }
}
