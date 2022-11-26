using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class handles the loot that will be droppped by the zombies.
/// </summary>
public class LootGenerator : MonoBehaviour
{
    public Transform lootParent;

    //Instantiate loot
    public GameObject food;
    public GameObject water;
    public GameObject medkit;
    public GameObject pills;
    public GameObject pistolAmmo;
    public GameObject uziAmmo;
    public GameObject riffleAmmo;

    private void Start()
    {
        lootParent = GameObject.Find("Loot").GetComponent<Transform>();
    }

    /// <summary>
    ///     This class will decide randomly what kind of loot will be instantiated.
    /// </summary>
    /// <param name="position"> Receives the position where the zombie died, so we can spawn the llot drop right there. </param>
    public void GenerateLoot(Vector3 position)
    {
        GameObject inst = null;
        int quantity = 0;
        int randomChoice = Random.Range(0, 100);
        bool willSpawn = true;

        // 35% No loot
        if(randomChoice >= 0 && randomChoice < 35)
            willSpawn = false;
        // 10% Pistol Ammo
        if (randomChoice >= 35 && randomChoice < 45)
        {
            inst = pistolAmmo;
            // How many items.
            int internalRandom = Random.Range(0, 10);
            if(internalRandom >= 0 && internalRandom < 8)
                quantity = 6;
            if (internalRandom == 8)
                quantity = 12;
            if (internalRandom == 9)
                quantity = 18;
        }
        // 10% Uzi Ammo
        if (randomChoice >= 45 && randomChoice < 55)
        {
            inst = uziAmmo;
            // How many items.
            int internalRandom = Random.Range(0, 10);
            if (internalRandom >= 0 && internalRandom < 9)
                quantity = 10;
            if (internalRandom == 9)
                quantity = 20;
        }
        // 10% Rifle Ammo
        if (randomChoice >= 55 && randomChoice < 65)
        {
            inst = riffleAmmo;
            // How many items.
            int internalRandom = Random.Range(0, 20);
            if (internalRandom >= 0 && internalRandom < 19)
                quantity = 15;
            if (internalRandom == 19)
                quantity = 30;
        }
        // 10% Water
        if (randomChoice >= 65 && randomChoice < 75)
        {
            inst = water;
            // How many items.
            int internalRandom = Random.Range(0, 10);
            if (internalRandom >= 0 && internalRandom < 8)
                quantity = 2;
            if (internalRandom == 8)
                quantity = 1;
            if (internalRandom == 9)
                quantity = 3;
        }
        // 10% Food
        if (randomChoice >= 75 && randomChoice < 85)
        {
            inst = food;
            // How many items.
            int internalRandom = Random.Range(0, 10);
            if (internalRandom >= 0 && internalRandom < 8)
                quantity = 2;
            if (internalRandom == 8)
                quantity = 1;
            if (internalRandom == 9)
                quantity = 3;
        }
        // 10% Pills
        if (randomChoice >= 85 && randomChoice < 95)
        {
            inst = pills;
            // How many items.
            int internalRandom = Random.Range(0, 10);
            if (internalRandom >= 0 && internalRandom < 4)
                quantity = 2;
            if (internalRandom >= 4 && internalRandom < 8)
                quantity = 1;
            if (internalRandom == 8 || internalRandom == 9)
                quantity = 3;
        }
        // 5% Medkit
        if (randomChoice >= 95 && randomChoice < 100)
        {
            inst = medkit;
            // How many items.
            int internalRandom = Random.Range(0, 10);
            if (internalRandom >= 0 && internalRandom < 6)
                quantity = 1;
            if (internalRandom >= 6 && internalRandom < 10)
                quantity = 2;
        }

        // If random loot is not nothing.
        if (willSpawn)
        {
            inst.GetComponent<DroppableItems>().quantity = quantity;
            position.y += 1;
            GameObject gameObject = Instantiate(inst, position, Quaternion.identity, lootParent);
        }
    }
}
