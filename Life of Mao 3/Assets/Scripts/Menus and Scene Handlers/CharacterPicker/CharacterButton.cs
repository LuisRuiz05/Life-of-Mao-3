using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Acts as the base for the character picking button.
/// </summary>
public class CharacterButton : MonoBehaviour
{
    public CharacterPicker characters;
    public CharacterStatsDisplay stats;

    public int index;

    /// <summary>
    ///     It will be the action executed with the button click and it will display the selected character.
    /// </summary>
    public void SetCharacter()
    {
        characters.DisplayCharacter(index);
        stats.UpdateStatistics(characters.charactersScriptable[index]);
    }
}
