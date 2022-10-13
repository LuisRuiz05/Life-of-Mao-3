using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class will receive the selected character's index, and will spawn it every time the main scene is loaded.
/// </summary>
public class LoadCharacter : MonoBehaviour
{
    public SettingsController settings;
    public GameObject[] players; // Will contain a list of all the character's prefabs.

    private void Awake()
    {
        settings = GameObject.Find("OnSceneChangeLoadObjects").GetComponent<SettingsController>();
        Instantiate(players[settings.selectedCharacter], new Vector3(0,2,0), Quaternion.identity);
    }
}
