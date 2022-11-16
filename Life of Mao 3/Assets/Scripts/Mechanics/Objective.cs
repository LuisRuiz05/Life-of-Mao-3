using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Controls the final objective from the level.
/// </summary>
public class Objective : MonoBehaviour
{
    public SettingsController controller;
    public Timer timer;

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Settings").GetComponent<SettingsController>();
        timer = GameObject.Find("UI/Timer").GetComponent<Timer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player enters the zone.
        if (other.gameObject.CompareTag("Player"))
        {
            // Get the game's data.
            controller.rewards.missionState = true;
            controller.rewards.time = timer.maxTime - timer.time;

            // Load rewards' scree.
            SceneManager.LoadScene(3);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
