using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieProceduralSpawner : MonoBehaviour
{
    // This script may be attached to player. (To confirm)
    public GameObject zombiePrefab;
    
    [Header("Lists")]
    Collider[] colliders = new Collider[100];
    public List<GameObject> zombiesInArea;
    public List<GameObject> spawnersInArea;

    [Header("Spawn Points")]
    public Transform spawnersParent;
    public Transform[] spawnPoints;

    public int maxZombiesInArea = 25;
    public ZombieReserve reserveScript;
    public LayerMask layer;

    private void Start()
    {
        // Instantitate lists.
        zombiesInArea = new List<GameObject>();
        spawnersInArea = new List<GameObject>();

        // Get all the spawners.
        spawnersParent = GameObject.Find("ZombieSpawner").transform;
        
        for (int i = 0; i < spawnersParent.childCount; i++)
        {
            spawnPoints[i] = spawnersParent.GetChild(i);
        }
    }

    private void Update()
    {
        Scan();
        //Fill();
    }

    private void Scan()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, 30, colliders, layer);

        zombiesInArea.Clear();
        spawnersInArea.Clear();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (obj.CompareTag("Enemy"))
            {
                zombiesInArea.Add(obj);
            }
            if (obj.CompareTag("Spawner"))
            {
                spawnersInArea.Add(obj);
            }
        }

        Debug.Log("In Area: " + zombiesInArea.Count);
        Debug.Log("Missing: " + (maxZombiesInArea - zombiesInArea.Count));
        Fill();
    }

    void Fill()
    {
        if(zombiesInArea.Count < maxZombiesInArea)
        {
            Transform pickedSpawn = spawnersInArea[Random.Range(0, spawnersInArea.Count)].transform;
            Instantiate(zombiePrefab, pickedSpawn);
        }   
    }
}
