using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class is in charge of spawning zombies while the game is running.
/// </summary>
public class ZombieProceduralSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    
    [Header("Lists")]
    Collider[] colliders = new Collider[100];
    public List<GameObject> zombiesInArea;
    public List<GameObject> spawnersInArea;

    [Header("Spawn Points")]
    public Transform spawnersParent;
    public Transform[] spawnPoints;

    private int maxZombiesInArea = 15;
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
    }

    /// <summary>
    ///     This function will scan the player's area in order to save the availabe spawners and zombies in area.
    /// </summary>
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

        //Debug.Log("In Area: " + zombiesInArea.Count);
        //Debug.Log("Missing: " + (maxZombiesInArea - zombiesInArea.Count));
        Fill();
    }

    /// <summary>
    ///     If there's a zombie missing in the area, it will be spawned in one of the available spawners.
    /// </summary>
    void Fill()
    {
        if(zombiesInArea.Count < maxZombiesInArea && spawnersInArea.Count > 0)
        {
            Transform pickedSpawn = spawnersInArea[Random.Range(0, spawnersInArea.Count)].transform;
            Instantiate(zombiePrefab, pickedSpawn);
        }   
    }
}
