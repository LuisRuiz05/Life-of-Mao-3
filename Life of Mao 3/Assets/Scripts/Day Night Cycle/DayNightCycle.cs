using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class will manage the game's time, rotation of sun, moon and clouds, and the skybox. 
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    public Light moon;

    public float timeSpeed;
    public float minutes;

    public float degrees;

    //public Material day;
    //public Material night;

    void Update()
    {
        minutes += timeSpeed * Time.deltaTime;
        if (minutes >= 1440) // As 1440 minutes represent 24 hours, we will restart the minutes counter each day.
        {
            RenderSettings.skybox.SetFloat("_Rotation", 0.0f);
            minutes = 0;
        }

        degrees = minutes / 4; // Sun will rotate in 1° every 4 minutes, so it can do it can complete 360° everyday.
        RenderSettings.skybox.SetFloat("_Rotation", degrees); // Will rotate clouds so they are not static.
        this.transform.localEulerAngles = new Vector3(degrees, -90f, 0f); 

        if (degrees >= 0 && degrees < 160) // DAY
        {
            this.GetComponent<Light>().enabled = true;
            RenderSettings.skybox.SetFloat("_Exposure", 1.05f);
            moon.enabled = false;
        }

        if (degrees >= 160 && degrees < 180) // DAY-NIGHT
        {
            moon.enabled = true;
            float interpolation = RenderSettings.skybox.GetFloat("_Exposure");
            if (RenderSettings.skybox.GetFloat("_Exposure") > 0.11f)
            {
                interpolation -= 0.025f * Time.deltaTime;
                RenderSettings.skybox.SetFloat("_Exposure", interpolation);
            };
        }

        if (degrees >= 180 && degrees < 340) // NIGHT
        {
            moon.enabled = true;
            RenderSettings.skybox.SetFloat("_Exposure", 0.11f);
            this.GetComponent<Light>().enabled = false;
        }

        if (degrees >= 340 && degrees < 360) // NIGHT-DAY
        {
            this.GetComponent<Light>().enabled = true;
            float interpolation = RenderSettings.skybox.GetFloat("_Exposure");
            if (RenderSettings.skybox.GetFloat("_Exposure") < 1.05f)
            {
                interpolation += 0.025f * Time.deltaTime;
                RenderSettings.skybox.SetFloat("_Exposure", interpolation);
            };
        }
    }
}
