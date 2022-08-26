using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     This class fixes an issue where everytime we get back to the main menu, we're not able to get back into the settings menu.
/// </summary>
public class MenuSettingsButtonUpdater : MonoBehaviour
{
    public Button backButton;
    public SettingsController optionController;

    void Start()
    {
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(SetButton);
        optionController = GameObject.Find("OnSceneChangeLoadObjects").GetComponent<SettingsController>();
    }

    /// <summary>
    ///     Looks for an object with the OptionController class, so we can actually call the settings interface from its method whenever the button is pressed.
    /// </summary>
    void SetButton()
    {
        if (optionController == null)
        {
            optionController = GameObject.Find("OnSceneChangeLoadObjects").GetComponent<SettingsController>();
        }
        optionController.MenuToSettingsButton();
    }
}
