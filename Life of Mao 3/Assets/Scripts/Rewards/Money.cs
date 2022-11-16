using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     This class will be in charge of storing money and manage money's different functions.
/// </summary>
public class Money : MonoBehaviour
{
    public Text moneyText;
    public int money;

    private void Awake()
    {
        money = PlayerPrefs.GetInt("money", 0);
    }

    /// <summary>
    ///     Display the available money at the character selection screen.
    /// </summary>
    public void DisplayMoney()
    {
        moneyText.text = money.ToString();
    }

    /// <summary>
    ///     Will add the reward money to the playerprefs.
    /// </summary>
    /// <param name="rewardMoney"> Will get the money to add. </param>
    public void GetMoney(int rewardMoney)
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) + rewardMoney);
        money = PlayerPrefs.GetInt("money");
    }
}
