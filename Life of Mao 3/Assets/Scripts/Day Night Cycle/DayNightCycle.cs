using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light moon;

    public float timeSpeed;
    public float minutes;

    public float degrees;

    void Update()
    {
        minutes += timeSpeed * Time.deltaTime;
        if (minutes >= 1440)
        {
            minutes = 0;
        }

        degrees = minutes / 4;
        this.transform.localEulerAngles = new Vector3(degrees, -90f, 0f);
        if (degrees >= 180)
        {
            this.GetComponent<Light>().enabled = false;
            moon.enabled = true;
        }
        else
        {
            this.GetComponent<Light>().enabled = true;
            moon.enabled = false;
        }

    }
}
