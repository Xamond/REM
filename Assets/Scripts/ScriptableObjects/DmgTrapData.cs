using UnityEngine;

[CreateAssetMenu(fileName = "New_DMG_Trap_Data", menuName = "DMG_Trap_Data")]
public class DmgTrapData : ScriptableObject
{
    [SerializeField]
    private Texture _texture;
    [SerializeField]
    private int damage;

    //Getter
    public Texture _Texture { get { return _texture; }}
    public int Damage { get { return damage;} }
}
