using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyCharacter : MonoBehaviour
{
    Character currentCharacter;

    public GameObject confirmationPanel;
    public Text price;
    public Button confirmationButton;

    public GameObject[] lockedCharacters;
    public Button[] buttons;
    public Money moneyScript;

    private void OnEnable()
    {
        foreach(GameObject character in lockedCharacters)
        {
            // Will check if the character is available, if it is, will make the locked button dissapear.
            Character stats = character.GetComponent<CharacterLockedButton>().character;
            int exists = PlayerPrefs.GetInt(stats.name, 0);
            if (exists != 0)
            {
                character.SetActive(false);
                buttons[stats.index - 6].interactable = true;
            }
        }
    }

    public void ShowConfirmation(Character charac)
    {
        currentCharacter = charac;

        int money = PlayerPrefs.GetInt("money", 0);
        price.text = "" + charac.price;

        confirmationPanel.SetActive(true);
        
        // If you don't have enough money, you can't confirm transaction.
        confirmationButton.interactable = money < charac.price ? false : true;
    }

    public void ConfirmTransaction()
    {
        int money = PlayerPrefs.GetInt("money");

        if (money > currentCharacter.price)
        {
            // Set character available
            currentCharacter.available = true;
            PlayerPrefs.SetInt(currentCharacter.name, 1);
            
            // Pay
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - currentCharacter.price);

            // Unlock character
            lockedCharacters[currentCharacter.index - 6].SetActive(false);
            buttons[currentCharacter.index - 6].interactable = true;

            // Finish transaction
            confirmationPanel.SetActive(false);
            moneyScript.money = PlayerPrefs.GetInt("money");
            moneyScript.DisplayMoney();
        }
    }

    public void DenyTransaction()
    {
        confirmationPanel.SetActive(false);
    }
}
