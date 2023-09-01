using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardObjPrefab;

    private string _cardDataPath;

    [SerializeField]
    private float _spacing = 1f;
    [SerializeField]
    private int _numRows = 4, _numCols = 4;
    [SerializeField]
    private float _marginX = 0, _marginY = 0;

    [SerializeField]
    private List<GameObject> cardObjsOnField = new List<GameObject>();
    private List<CardData> cardDatas = new List<CardData>();

    private bool anyCardsLeft;

    private void Awake()
    {
        InitializeCardData(_cardDataPath);
        GenerateCards(_marginX, _marginY);
        InitializePairs();
    }

    private void GenerateCards(float marginX, float marginY)
    {
        for(int row = 0; row < _numRows; row++)
        {
            for (int col = 0; col < _numCols; col++)
            {
                Vector3 cardPosition = CalculateCardPos(col, row, marginX, marginY);
                Quaternion cardRotation = Quaternion.Euler(270f, -180f, 180f);
                GameObject newCardObj = Instantiate(_cardObjPrefab, cardPosition, cardRotation);
                SetCardObjName(newCardObj, row, col);
                cardObjsOnField.Add(newCardObj);
            }
        }
    }

    /// <summary>
    /// Refreshes the card game with new values from the input stage data.
    /// Generates a new set of cards based on the updated values 
    /// of numRows, numCols, marginX, marginY, and cardDataPath.
    /// Initializes the card data and pairs for the new set of cards.
    /// </summary>
    /// <param name="inpStageData">The input stage data containing the updated values.</param>
    public void RefreshCards(StageData inpStageData)
    {
        this._numRows = inpStageData.NumRows;
        this._numCols = inpStageData.NumCols;
        this._marginX = inpStageData.MarginX;
        this._marginY = inpStageData.MarginY;
        this._cardDataPath = inpStageData.CardDataFolderPath;

        GenerateCards(_marginX, _marginY);
        InitializeCardData(_cardDataPath);
        InitializePairs();
    }

    private void SetCardObjName(GameObject inpCard, int row, int col)
    {
        inpCard.name = "Card " + row + " " + col;
    }

    private Vector3 CalculateCardPos(int col, int row, float marginX, float marginY)
    {
        float xPos = CalculateAxisValue(col, _spacing, marginX);
        float zPos = CalculateAxisValue(row, -_spacing, marginY);
        return new Vector3(xPos, 0.01f, zPos);
    }

    private float CalculateAxisValue(int posIndex, float spacing, float margin)
    {
        return (posIndex * spacing) + margin;
    }

    public void FlipCard(Card inpCard)
    {
        inpCard.FlipCard();
    }


    public void FlipCards(Card[] inpCards)
    {
        for (int i = 0; i < inpCards.Length; i++)
        {
            inpCards[i].FlipCard();
        }
    }

    public void SaveCardObjPositions(List<GameObject> cardObjsOnField, List<Vector3> allPositions)
    {
        cardObjsOnField.ForEach(cardObj => allPositions.Add(cardObj.transform.position));
    }

    public void ShuffleCards(List<GameObject> cardObjsOnField, List<Vector3> allPositions)
    {
        System.Random randomNumber = new System.Random();
        allPositions = allPositions.OrderBy(position => randomNumber.Next()).ToList();

        for(int i = 0; i < cardObjsOnField.Count; i++)
        {
            cardObjsOnField[i].transform.position = allPositions[i];
        }
    }

    private void InitializeCardData(string folder)
    {
        cardDatas = Resources.LoadAll<CardData>(folder).ToList();
    }

    private void InitializePairs()
    {
        List<CardData> shuffledCardData = cardDatas.OrderBy(cardData => Random.value).ToList();
        List<CardData> cardDataToAssign = new List<CardData>();

        for (int i = 0; i < cardObjsOnField.Count / 2; i++)
        {
            cardDataToAssign.Add(shuffledCardData[i]);
            cardDataToAssign.Add(shuffledCardData[i]);
        }

        cardDataToAssign = cardDataToAssign.OrderBy(cardData => Random.value).ToList();

        for (int i = 0; i < cardObjsOnField.Count; i++)
        {
            AssignCardData(cardObjsOnField[i], cardDataToAssign[i]);
        }
    }

    private void AssignCardData(GameObject cardObj, CardData cardData)
    {
        Card card = cardObj.GetComponent<Card>();
        Renderer cardFaceRenderer = cardObj.GetComponent<Renderer>();

        card.Identifier = cardData.Id;
        cardFaceRenderer.material.mainTexture = cardData.FaceTexture;        
    }

    //Getter und Setter
    public List<GameObject> CardObjsOnField
    {
        get { return cardObjsOnField; }
        set { cardObjsOnField = value; }
    }

    public bool AnyCardsLeft
    {
        get{return anyCardsLeft;}
        set{anyCardsLeft = value;}
    }

    public float Spacing { set { _spacing = value;} }
    public int NumRows { set { _numRows = value; } }
    public int NumCols { set { _numCols = value; } }
    public float MarginX { set { _marginX = value; } }
    public float MarginY { set { _marginY = value; } }
    public string CardDataPath { set { _cardDataPath = value; } }
}
