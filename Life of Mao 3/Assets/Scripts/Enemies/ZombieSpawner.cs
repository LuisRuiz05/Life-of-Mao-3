using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombieParent;
    public GameObject zombie;

    private void Awake()
    {
        zombieParent = GameObject.Find("ZombieParent");
    }

    public void SpawnZombie()
    {
        GameObject zombieClone = Instantiate(zombie, gameObject.transform);
    }

}
