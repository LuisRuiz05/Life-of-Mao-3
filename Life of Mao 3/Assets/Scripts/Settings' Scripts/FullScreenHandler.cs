using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Manages the game's resolution and if the screen's full or not.
/// </summary>
public class FullScreenHandler : MonoBehaviour
{
    public Toggle toggle;
    public Dropdown dropdown;
    Resolution[] resolutions;

    void Start()
    {
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        } else
        {
            toggle.isOn = false;
        }
        CheckResolution();
    }

    /// <summary>
    ///     Update's the full or non-full screen according to the selected option.
    /// </summary>
    public void ChangeScreenSize(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    /// <summary>
    ///     Resets the previous resolution options so the game will look for the available resolutions in the computer. It'll save the actual screen resolution.
    /// </summary>
    public void CheckResolution()
    {
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        int actualResolution = 0;

        // After resetting the resolution options, the game will look for the available resolutions in the computer and add them to the updated option list.
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                actualResolution = i;
            }
        }
        dropdown.AddOptions(options);
        dropdown.value = actualResolution;
        dropdown.RefreshShownValue();

        PlayerPrefs.GetInt("resolution", 0);
    }

    /// <summary>
    ///      Updates the game's resolution according to the new option selected at the dropdown.
    /// </summary>
    public void ChangeResolution(int indexResolution)
    {
        PlayerPrefs.SetInt("resolution", dropdown.value);
        Resolution resolution = resolutions[indexResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
