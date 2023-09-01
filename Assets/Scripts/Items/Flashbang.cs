using System.Collections;
using UnityEngine;

public class Flashbang : Item
{
    /// <summary>
    /// This method checks if the player has enough money to use a flashbang 
    /// and then flips all trap cards on the field for a short amount of time.
    /// </summary>
    /// <param name="price">The price to use the item.</param>
    public void UseFlashbang(int price)
    {
        PlayerStat playerFound = FindObjectOfType<PlayerStat>();

        if(playerFound.CurrentMoney >= price && canUse)
        {
            base.SubtractMoney(price);
            _audioSource.PlayOneShot(sfx);
            FlipMultipleCards();
        }
    }

    /// <summary>
    /// Starts a coroutine to flip all trap cards on the field after a delay.
    /// </summary>
    private void FlipMultipleCards()
    {
        StartCoroutine(FlipCardsDelayed(1.6f, 1.4f));
    }

    /// <summary>
    /// Flips a card if it is a trap card.
    /// </summary>
    /// <param name="inpCard">The card to flip.</param>
    private void FlipTrap(Card inpCard)
    {
        if (inpCard.IsTrap)
            inpCard.FlipCard();
    }

    // <summary>
    // Flips all trap cards on the field.
    // </summary>
    private void FlipAllTraps()
    {
        cardObjsOnField.ForEach(cardObj => FlipTrap(cardObj.GetComponent<Card>()));
    }

    ///<summary>
    ///Flips all trap cards on the field face-up after a specified delay time, then flips them back face-down after another specified delay time.
    ///</summary>
    ///<param name="flipUpDelayTime">The delay time before the trap cards are flipped face-up.</param>
    ///<param name="flipBackDelayTime">The delay time before the trap cards are flipped back face-down.</param>
    IEnumerator FlipCardsDelayed(float flipUpDelayTime, float flipBackDelayTime)
    {
        canUse = false;

        yield return new WaitForSeconds(flipUpDelayTime);

        FlipAllTraps();

        yield return new WaitForSeconds(flipBackDelayTime);

        FlipAllTraps();

        canUse = true;
    }
}
