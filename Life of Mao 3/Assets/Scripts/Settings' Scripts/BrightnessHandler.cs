using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///      Manages the game's brightness.
/// </summary>
public class BrightnessHandler : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image brightnessPanel;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("brightness", 0.5f);
        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, slider.value);
    }

    /// <summary>
    ///      Updates the game's brightness according to the input in the slider.
    /// </summary>
    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("brightness", sliderValue);
        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, slider.value);
    }
}
