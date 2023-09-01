using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField]
    protected TrapType type;

    public abstract void ActivateTrap();

    //Getter
    public TrapType Type { get { return type; } }
}

public enum TrapType
{
    BEARTRAP,
    TIMEBOMB,
    TIMETRAP
}
