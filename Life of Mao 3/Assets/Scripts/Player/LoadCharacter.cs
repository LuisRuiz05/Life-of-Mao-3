using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public SettingsController settings;
    public GameObject[] players;

    private void Awake()
    {
        settings = GameObject.Find("OnSceneChangeLoadObjects").GetComponent<SettingsController>();
        Instantiate(players[settings.selectedCharacter]);
    }
}
