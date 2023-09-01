using UnityEngine;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Card Data")]
public class CardData : ScriptableObject
{
    [SerializeField]
    private int id;
    [SerializeField]
    private Texture faceTexture;

    public int Id
    {
        get { return id; }
    }

    public Texture FaceTexture
    {
        get { return faceTexture; }
    }
}
