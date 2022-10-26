using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Scriptable object acting as a base for the final reward screen.
/// </summary>
[CreateAssetMenu(fileName = "Reward System")]
public class RewardsLoader : ScriptableObject
{
    public bool missionState;
    public float time;
    public int zombiesKilled;
}
