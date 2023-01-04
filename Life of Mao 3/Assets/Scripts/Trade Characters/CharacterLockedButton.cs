using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLockedButton : MonoBehaviour
{
    public BuyCharacter transaction;
    public Character character;

    public void Click()
    {
        transaction.ShowConfirmation(character);
    }
}
