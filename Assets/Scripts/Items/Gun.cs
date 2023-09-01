using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{
    /// <summary>
    /// Shoots a random card from the field if player has enough money and can use it
    /// </summary>
    /// <param name="price"></param> the cost of shooting a card
    public void Shoot(int price)
    {
        PlayerStat _playerStat = FindObjectOfType<PlayerStat>();

        if(_playerStat.CurrentMoney >= price && canUse)
        {
            base.SubtractMoney(price);
            DestroyRandomCards();
        }
    }

    /// <summary>
    /// Starts the coroutine RndCardDestructionDelayed to destroy random cards from the field
    /// </summary>
    private void DestroyRandomCards()
    {
        StartCoroutine(RndCardDestructionDelayed(1f, 0.4f));
    }

    /// <summary>
    /// Destroys a random card from the field and checks if it is a trap card
    /// </summary>
    /// <param name="card"></param> the card to be destroyed
    /// <param name="randomIndex"></param> a random index used to select a card from the list
    private void DestroyRandomCard(Card card, int randomIndex)
    {
        while (card.IsTrap)
        {
            randomIndex = Random.Range(0, cardObjsOnField.Count - 1);
            card = cardObjsOnField[randomIndex].GetComponent<Card>();
        }

        _audioSource.PlayOneShot(sfx);
        cardObjsOnField.Remove(card.gameObject);
        Destroy(card.gameObject);
    }

    /// <summary>
    /// Destroys all cards that have the same identifier as the card provided
    /// </summary>
    /// <param name="card"></param> the card to be compared to other cards on the field
    private void DestroyMatchingCards(Card card)
    {
        for (int i = 0; i < cardObjsOnField.Count; i++)
        {
            if (card.Identifier == cardObjsOnField[i].GetComponent<Card>().Identifier)
            {
                _audioSource.PlayOneShot(sfx);
                Card card2 = cardObjsOnField[i].GetComponent<Card>();
                cardObjsOnField.Remove(card2.gameObject);
                Destroy(card2.gameObject);
            }
        }
    }

    /// <summary>
    /// Destroys a random card from the field, waits for 1 second, 
    /// and then destroys all matching cards.
    /// Uses the DestroyRandomCard and DestroyMatchingCards methods
    /// </summary>
    /// <returns>Returns an IEnumerator object for use in a coroutine</returns>
    IEnumerator RndCardDestructionDelayed(float destroyDelayTime, float reuseDelayTime)
    {
        int randomIndex = Random.Range(0, cardObjsOnField.Count - 1);
        Card card = cardObjsOnField[randomIndex].GetComponent<Card>();

        canUse = false;

        DestroyRandomCard(card, randomIndex);
        yield return new WaitForSeconds(destroyDelayTime);
        DestroyMatchingCards(card);
        yield return new WaitForSeconds(reuseDelayTime);
        canUse = true;
    }
}
