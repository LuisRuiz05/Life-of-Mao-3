using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Scriptable object acting as a base for the each character in the game.
/// </summary>
[CreateAssetMenu(fileName = "New Character")]
public class Character : ScriptableObject
{
    // General atributes
    public string name;
    public Sprite icon;
    public int index;
    public enum gender
    {
        masculine,
        feminine
    }
    public gender characterGender;

    // Statistics
    public int strength;
    public int speed;
    public int stamina;
    public int health;
}
