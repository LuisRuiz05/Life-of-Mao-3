using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Starts a countdown for loading the main menu, then it keeps the music playing at main menu.
/// </summary>
public class SplashScreen : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Invoke("LoadMainMenu", 13.8f);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }

}
