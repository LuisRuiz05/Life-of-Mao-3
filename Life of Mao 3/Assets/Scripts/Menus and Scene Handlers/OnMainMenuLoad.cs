using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Looks for the music component, if it exists, we ignore it, if it doesn't, then we play the music from the AudioSource.
/// </summary>
public class OnMainMenuLoad : MonoBehaviour
{
    AudioSource audio;

    private void Awake()
    {
        var dontDestroy = GameObject.Find("Music");
        if (dontDestroy == null)
        {
            audio = GetComponent<AudioSource>();
            audio.Play();
        }
    }
}
