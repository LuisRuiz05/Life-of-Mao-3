using UnityEngine;

/// <summary>
///     Scriptable object acting as a base for the each item in the game.
/// </summary>
[CreateAssetMenu(fileName = "New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    [Range(1,64)]public int maxStackSize = 64;
}
