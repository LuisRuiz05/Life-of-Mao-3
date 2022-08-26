using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Manages the game's master volume.
/// </summary>
public class VolumeHandler : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image muteImg;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volumeAudio", 0.5f);
        AudioListener.volume = slider.value;
        CheckMute();
    }

    /// <summary>
    ///     Adjusts the game's volume to the input in the slider.
    /// </summary>
    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("volumeAudio", sliderValue);
        AudioListener.volume = slider.value;
        CheckMute();
    }

    /// <summary>
    ///     Checks if the game's volume equals 0, and according to this information, the icon's color will change.
    /// </summary>
    public void CheckMute()
    {
        if (sliderValue == 0)
        {
            muteImg.color = Color.red;
        }
        else
        {
            muteImg.color = Color.gray;
        }
    }

    /// <summary>
    ///     If game's volume is equals 0, it will modify it to 50. If game's volume is higher than 0, it will take it back to 0.
    /// </summary>
    public void Mute()
    {
        if (sliderValue == 0)
        {
            slider.value = 0.5f;
            sliderValue = 0.5f;
            PlayerPrefs.SetFloat("volume", sliderValue);
            AudioListener.volume = slider.value;
            CheckMute();
        }
        else
        {
            slider.value = 0;
            sliderValue = 0;
            PlayerPrefs.SetFloat("volume", sliderValue);
            AudioListener.volume = slider.value;
            CheckMute();
        }
    }
}
