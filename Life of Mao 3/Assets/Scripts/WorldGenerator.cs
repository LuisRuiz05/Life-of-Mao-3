using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class WorldGenerator : MonoBehaviour
{
    [Range(1,5)]
    public int citySize;
    public RunTimeSample generator;
    public GameObject city;
    public NavMeshSurface surface;
    public GameObject[] spawners;

    public int maxZombies = 100;
    private int currentZombies = 0;
    private int spawnNum;

    void Awake()
    {
        generator.GenerateCityAtRuntime(citySize);
        generator.GenerateBuildings();
        
        city = GameObject.Find("City-Maker");
        Debug.Log("Applying changes");
        city.layer = LayerMask.NameToLayer("Map");

        for (int i = 0; i < city.transform.childCount; i++)
        {
            city.transform.GetChild(i).gameObject.isStatic = true;
            city.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Map");
        }

        surface.BuildNavMesh();
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        spawnNum = spawners.Length;

        for(int i = 0; i < maxZombies; i++)
        {
            Debug.Log("Spawning");
            spawners[Random.Range(0, spawnNum)].GetComponent<ZombieSpawner>().SpawnZombie();
        }
    }
}
