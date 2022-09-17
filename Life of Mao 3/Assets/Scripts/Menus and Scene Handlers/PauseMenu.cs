using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Manages the pause menu and all of its functions.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    static bool isPaused = false;
    public SettingsController settings;
    public GameObject pauseUI;
    public CinemachineInputProvider cinemachineAim;
    public CinemachineInputProvider cinemachineNormal;
    public GameObject settingsUI;
    //public AudioSource music;

    private PlayerInput playerInput;
    private InputAction pauseAction;
    private InputAction lookAction;

    void Start()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        cinemachineAim = GameObject.Find("ThirdPersonCamera").GetComponent<CinemachineInputProvider>();
        cinemachineNormal = GameObject.Find("AimCamera").GetComponent<CinemachineInputProvider>();
        settings = GameObject.Find("OnSceneChangeLoadObjects").GetComponent<SettingsController>();

        settingsUI = settings.screenOptions;
        pauseAction = playerInput.actions["Pause"];

        Resume();
    }

    void Update()
    {
        // Whenever the Escape key is pressed, the menu's state will change, except if the settings screen is actually shown.
        if (pauseAction.triggered)
        {
            if (isPaused && !settingsUI.activeInHierarchy)
            {
                Resume();
            }
            else
            {
                if (!settingsUI.activeInHierarchy)
                {
                    Pause();
                }
            }
        }
    }

    /// <summary>
    ///     Ends the pause.
    /// </summary>
    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cinemachineNormal.XYAxis.action.Enable();
        cinemachineAim.XYAxis.action.Enable();
        //music.Play();
    }

    /// <summary>
    ///     Pauses the game.
    /// </summary>
    public void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cinemachineNormal.XYAxis.action.Disable();
        cinemachineAim.XYAxis.action.Disable();
        //music.Pause();
    }

    /// <summary>
    ///     Loads the main menu scene.
    /// </summary>
    public void Menu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    ///     Shows the settings screen.
    /// </summary>
    public void Settings()
    {
        pauseUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    /// <summary>
    ///     Returns if the game is paused or not.
    /// </summary>
    public bool IsPaused()
    {
        return isPaused;
    }

    /// <summary>
    ///     Closes the game.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
