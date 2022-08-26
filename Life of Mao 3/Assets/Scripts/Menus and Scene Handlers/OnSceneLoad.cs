using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Keeps necessary information for posterior scenes.
/// </summary>
public class OnSceneLoad : MonoBehaviour
{
    // Looks for a OnSceneChangeLoader object type at scene, if it doesn't exist, we keep this GameObject, but if we have one already, we will keep the old one and destroy this repeated GameObject.
    private void Awake()
    {
        var dontDestroy = FindObjectsOfType<OnSceneLoad>();
        if(dontDestroy.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
