using UnityEngine;

public enum CardDirection
{
    UP,
    DOWN
}

public class Card : MonoBehaviour
{
    [SerializeField]
    private int identifier;
    [SerializeField]
    private bool isTrap;
    [SerializeField]
    private Trap attachedTrap = null;

    private CardDirection currentCardDirection = CardDirection.DOWN;

    private float targetHeight = 0.01f, targetRotation = 270f;

    private void Update()
    {
        UpdateHeight();
        UpdateRotation();
    }

    /// <summary>
    /// Called when the mouse button is pressed on the card.
    /// </summary>
    private void OnMouseDown()
    {
        FindObjectOfType<GameManager>().OnCardClicked(this);
    }

    /// <summary>
    /// Flips the card depending on the direction it is facing.
    /// </summary>
    public void FlipCard()
    {
        if (currentCardDirection == CardDirection.DOWN)
        {
            targetHeight = 0.4f;
            targetRotation = 90f;
            currentCardDirection = CardDirection.UP;
        }
        else if(currentCardDirection == CardDirection.UP)
        {
            targetHeight = 0.01f;
            targetRotation = 270f;
            currentCardDirection = CardDirection.DOWN;
        }
    }

    /// <summary>
    /// Updates the position of the card to move it towards its target height value.
    /// </summary>
    private void UpdateHeight()
    {
        float heightValue = Mathf.MoveTowards(transform.position.y, targetHeight, 0.5f * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, heightValue, transform.position.z);
    }

    /// <summary>
    /// Updates the rotation of the card to move it towards its target rotation value.
    /// </summary>
    private void UpdateRotation()
    {
        Quaternion rotationValue = Quaternion.Euler(targetRotation, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationValue, 10f * Time.deltaTime);
    }


    //Getter & Setter
    public bool IsTrap
    {
        get { return isTrap; }
        set { isTrap = value; }
    }

    public int Identifier
    {
        get{ return identifier; }
        set{ identifier = value; }
    }

    public Trap AttachedTrap
    {
        get { return attachedTrap; }
        set { attachedTrap = value; }
    }
}
