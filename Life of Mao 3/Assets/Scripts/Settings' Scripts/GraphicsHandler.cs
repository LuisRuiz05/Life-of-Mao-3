using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Manages the game's quality.
/// </summary>
public class GraphicsHandler : MonoBehaviour
{
    public Dropdown dropdown;
    public int quality;

    // Sets an initial quality in "high" or recovers the last quality set by the user.
    void Start()
    {
        quality = PlayerPrefs.GetInt("quality", 3);
        dropdown.value = quality;
        AdjustQuality();
    }

    /// <summary>
    ///     Updates the game's quality according to the new option selected at the dropdown.
    /// </summary>
    public void AdjustQuality()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("quality", dropdown.value);
        quality = dropdown.value;
    }
}
