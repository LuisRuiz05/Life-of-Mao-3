using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Counts the remaining time in the level.
/// </summary>
public class Timer : MonoBehaviour
{
    public float maxTime = 300;
    public float time;
    public Text timeText;
    public GameObject objective;

    public GameObject arrow;
    bool hasSpawnedArrow = false;

    private void Start()
    {
        time = maxTime;
    }

    void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
        }
        DisplayTime(time);
    }

    /// <summary>
    ///     Displays the remaining time in the UI.
    /// </summary>
    /// <param name="time"> Represents current time. </param>
    public void DisplayTime(float time)
    {
        if(time < 0)
        {
            time = 0;
            objective.SetActive(true);
            // Spawns a compass' arrow to give the player to the objective,
            if (!hasSpawnedArrow)
            {
                GameObject clone = Instantiate(arrow, GameObject.FindGameObjectWithTag("Player").transform);
                clone.GetComponent<Arrow>().objective = objective.transform;
                hasSpawnedArrow = true;
            }
        }

        float mins = Mathf.FloorToInt(time / 60);
        float secs = Mathf.FloorToInt(time % 60);

        timeText.text = string.Format("{0:00}:{1:00}", mins, secs);
    }
}
