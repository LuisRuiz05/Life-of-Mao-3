using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Image splashScreen;

    private void Start()
    {
        StartCoroutine(TurnScreenOff());
    }

    IEnumerator TurnScreenOff()
    {
        yield return new WaitForSeconds(15.8f);
        splashScreen.enabled =false;
    }
}
