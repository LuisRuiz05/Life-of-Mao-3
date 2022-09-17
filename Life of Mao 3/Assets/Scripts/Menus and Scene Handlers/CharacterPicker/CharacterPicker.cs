using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class makes the 
/// </summary>
public class CharacterPicker : MonoBehaviour
{
    public CharacterStatsDisplay stats;

    public GameObject menuScreen;
    public GameObject pickCharacterCanvas;
    public SettingsController settings;

    public GameObject[] characters;
    public Character[] charactersScriptable;

    public void Start()
    {
        settings = GameObject.Find("OnSceneChangeLoadObjects").GetComponent<SettingsController>();
        stats.UpdateStatistics(charactersScriptable[settings.selectedCharacter]);
        DisplayCharacter(settings.selectedCharacter);
    }

    /// <summary>
    ///     Shows the menu to pick the character.
    /// </summary>
    public void SelectCharacterButton()
    {
        menuScreen.SetActive(false);
        pickCharacterCanvas.SetActive(true);

        // Reset all the model's position.
        foreach (GameObject character in characters)
        {
            character.transform.localPosition = new Vector3(224, -89, 11);
            character.transform.rotation = Quaternion.Euler(0, 220, 0);
        }
    }

    /// <summary>
    ///     Goes back to menu after choosing a character.
    /// </summary>
    public void SelectCharacterToMenu()
    {
        pickCharacterCanvas.SetActive(false);
        menuScreen.SetActive(true);
    }

    /// <summary>
    ///     Displays the selected character in screen and sets the actual character index in the settings script;
    /// </summary>
    /// <param name="index"> Gets the index of the selected button. </param>
    public void DisplayCharacter(int index)
    {
        foreach(GameObject character in characters)
        {
            character.SetActive(false);
        }
        characters[index].transform.localPosition = new Vector3(224, -89, 11);
        characters[index].transform.rotation = Quaternion.Euler(0, 220, 0);
        characters[index].SetActive(true);
        settings.selectedCharacter = index;
    }
}
