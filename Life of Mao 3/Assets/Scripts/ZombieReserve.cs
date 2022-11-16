using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieReserve : MonoBehaviour
{
    public GameObject[] zombies;

    private void Start()
    {
        //SpawnReserve();
    }

    void SpawnReserve()
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject zombie = Instantiate(zombies[i], transform);
            zombie.SetActive(false);
        }
    }
}
