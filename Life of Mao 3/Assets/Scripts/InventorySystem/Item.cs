using UnityEngine;

/// <summary>
///     Scriptable object acting as a base for the each item in the game.
/// </summary>
[CreateAssetMenu(fileName = "New Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Pistol,
        Uzi,
        Rifle,
        Ammo,
        Consumable
    }

    public string itemName;
    public Sprite itemIcon;
    [Range(1,32)]public int maxStackSize = 15;
    public ItemType type;
    public bool isSpameable;
}
