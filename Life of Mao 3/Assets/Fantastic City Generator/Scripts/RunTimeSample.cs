using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTimeSample : MonoBehaviour
{

    public GameObject cg;
    public GameObject ts;

    public CityGenerator generator;
    private TrafficSystem trafficSystem;

    private bool withDownTownArea = true;
    private bool rightHand = true;

    void Awake()
    {
        generator = cg.GetComponent<CityGenerator>();
        Debug.Log(generator);
        generator.gameObject.isStatic = true;
    }
    public void GenerateCityAtRuntime(int citySize)
    {
        Destroy(GameObject.Find("CarContainer"));

        generator = cg.GetComponent<CityGenerator>();

        //generator.GenerateCity(citySize); // (city size:  1 , 2, 3 or 4) 


    }

    public void WithDownTownArea(bool value)
    {
        withDownTownArea = value;
    }
    public void RightHand(bool value)
    {
        rightHand = value;
    }
    

    public void GenerateBuildings()
    {
        float downTownSize = 100;
        Debug.Log(generator);
        generator.GenerateAllBuildings(withDownTownArea, downTownSize); // (skyscrappers: true or false)

    }


    public void AddTrafficSystem()
    {


        trafficSystem = ts.GetComponent<TrafficSystem>();

        trafficSystem.LoadCars((rightHand) ? 0 : 1);

        Debug.Log("Move the camera to the streets so that vehicles are generated around it");

   
    }


}
