using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private List<Image> images;
    [SerializeField]
    private List<Item> items;

    private void Update()
    {
        CheckAndAdjustVisibility();
    }

    /// <summary>
    /// This method checks and adjusts the visibility of items based on the player's current money. 
    /// </summary>
    public void CheckAndAdjustVisibility()
    {
        PlayerStat _playerStat = FindObjectOfType<PlayerStat>();
        int currentMoney = _playerStat.CurrentMoney;

        for(int i = 0; i < items.Count; i++) 
        {
            AdjustItemVisibility(currentMoney, items[i].Price, images[i]);
        }
    }

    ///<summary>
    /// Adjusts the visibility of each item's image based on the current player's money.
    ///</summary>
    ///<param name="currentMoney">The amount of money the player currently has.</param>
    ///<param name="price">The price of the item whose image visibility is being adjusted.</param>
    ///<param name="image">The image of the item whose visibility is being adjusted.</param>
    private void AdjustItemVisibility(int currentMoney, int price, Image image)
    {
        if(currentMoney >= price)
        {
            SetOpacity(image, 1f);
        }
        if(currentMoney < price)
            SetOpacity(image, 0.5f);
    }

    ///<summary>
    /// Sets the opacity of an image to a given value.
    ///</summary>
    ///<param name="image">The image whose opacity is being set.</param>
    ///<param name="opacityValue">The value to which the opacity of the image is being set.</param>
    public void SetOpacity(Image image, float opacityValue)
    {
        Color tmp = image.color;
        tmp.a = opacityValue;
        image.color = tmp;
    }
}
