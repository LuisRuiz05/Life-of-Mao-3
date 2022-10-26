using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Is in charge of loading the rewards' screen data and set it into the UI.
/// </summary>
public class Rewards : MonoBehaviour
{
    public RewardsLoader rewards;

    [Header("Data")]
    public bool missionState;
    public float time;
    public int zombiesKilled;
    public int money;

    [Header("UI References")]
    public Text stateText;
    public Text timeText;
    public Text zombiesText;
    public Text moneyText;

    void Start()
    {
        rewards = GameObject.FindWithTag("Settings").GetComponent<SettingsController>().rewards;

        // Load data.
        missionState = rewards.missionState;
        time = rewards.time;
        zombiesKilled = rewards.zombiesKilled;
        // Calculate earned money.
        money = (int)time + (zombiesKilled*2);

        // Convert time
        float mins = Mathf.FloorToInt(time / 60);
        float secs = Mathf.FloorToInt(time % 60);

        // Set data.
        if (missionState)
            stateText.text = "Succesful";
        else
            stateText.text = "Failed";
       
        timeText.text = string.Format("{0:00}:{1:00}", mins, secs);
        zombiesText.text = "" + zombiesKilled;
        moneyText.text = "" + money;
    }

}
