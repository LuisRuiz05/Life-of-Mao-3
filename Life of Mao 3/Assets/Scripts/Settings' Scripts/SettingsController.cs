using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Saves the settings interface GameObject so it can be used in differente scenes.
///     Manages functions used by most of the game's buttons.
/// </summary>
public class SettingsController : MonoBehaviour
{
    public GameObject screenOptions;
    public GameObject menuScreen;

    public PauseMenu pauseMenu;
    private void Start()
    {
        menuScreen = GameObject.Find("Menu");
    }

    /// <summary>
    ///     Shows the settings screen from main menu.
    /// </summary>
    public void MenuToSettingsButton()
    {
        if (menuScreen == null)
        {
            menuScreen = GameObject.Find("Menu");
        }
        menuScreen.SetActive(false);
        screenOptions.SetActive(true);
    }

    /// <summary>
    ///     Closes the settings screen and returns player to the previous screen.
    /// </summary>
    public void SettingsBackButton()
    {
        int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        // If the loaded scene is the main menu, it'll return player back to the menu.
        if (sceneIndex == 0)
        {
            if (menuScreen == null)
            {
                menuScreen = GameObject.Find("Menu");
            }
            menuScreen.SetActive(true);
            screenOptions.SetActive(false);
        }
        // If the loaded scene is the game, it'll return player back to the pause menu.
        else
        {
            if (pauseMenu == null)
            {
                pauseMenu = GameObject.Find("UI").GetComponent<PauseMenu>();
            }
            screenOptions.SetActive(false);
            pauseMenu.pauseUI.SetActive(true);
        }
    }
}
