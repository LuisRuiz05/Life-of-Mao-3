using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Displays player's stats in the select character's menu, according to the character's scriptable object.
/// </summary>
public class CharacterStatsDisplay : MonoBehaviour
{
    public Text name;
    public Slider strength;
    public Slider speed;
    public Slider stamina;
    public Slider health;

    int maxStats = 99;

    public void UpdateStatistics(Character character)
    {   
        name.text = character.name;
        strength.value = (float)character.strength / maxStats;
        speed.value = (float)character.speed / maxStats;
        stamina.value = (float)character.stamina / maxStats;
        health.value = (float)character.health / maxStats;
    }
}
