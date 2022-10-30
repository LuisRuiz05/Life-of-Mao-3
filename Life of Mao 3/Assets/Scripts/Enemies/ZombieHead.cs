using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Acts an intermediary with the zombie's AI when it receives a headshot.
/// </summary>
public class ZombieHead : MonoBehaviour
{
    public ZombieAI ai;

    void Start()
    {
        ai = gameObject.GetComponentInParent<ZombieAI>();
    }
}
