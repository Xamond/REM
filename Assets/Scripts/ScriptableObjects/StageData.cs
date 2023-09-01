using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage Data", menuName = "Stage Data")]
public class StageData : ScriptableObject
{
    [SerializeField]
    private float spacing;
    [SerializeField]
    private int numRows, numCols;
    [SerializeField]
    private string cardDataFolderPath;
    [SerializeField]
    private float marginX, marginY;

    public float Spacing { get { return spacing; } }
    public int NumRows { get { return numRows; } }
    public int NumCols { get { return numCols; } }
    public string CardDataFolderPath { get { return cardDataFolderPath; } }
    public float MarginX { get { return marginX; } }
    public float MarginY { get { return marginY; } }

}
