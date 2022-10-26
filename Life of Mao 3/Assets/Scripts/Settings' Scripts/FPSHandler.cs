using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
///     Manages the game's FPS counter.
/// </summary>
public class FPSHandler : MonoBehaviour
{
    private int avgFrameRate;
    public Toggle toggle;
    public Text fpsCounter;

    private void Start()
    {
        toggle.isOn = PlayerPrefs.GetInt("fps", 1) != 0;
        if (SceneManager.GetActiveScene().buildIndex == 2)
            fpsCounter = GameObject.Find("UI/FPSCounter").GetComponent<Text>();
    }

    public void ShowFPS(bool willShow)
    {
        PlayerPrefs.SetInt("fps", Convert.ToInt32(willShow));
        if (SceneManager.GetActiveScene().buildIndex == 2)
            fpsCounter.enabled = willShow;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            if(fpsCounter == null)
            {
                fpsCounter = GameObject.Find("UI/FPSCounter").GetComponent<Text>();

                if (PlayerPrefs.GetInt("fps", 1) == 1)
                    fpsCounter.enabled = true;
                else
                    fpsCounter.enabled = false;
            }

            float current = 0;
            current = 1f / Time.unscaledDeltaTime;
            avgFrameRate = (int)current;
            fpsCounter.text = avgFrameRate + "fps";
        }
    }
}
