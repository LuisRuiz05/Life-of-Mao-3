using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFixer : MonoBehaviour
{
    public GameObject[] zombies;

    void Awake()
    {
        Debug.Log("Child count: " + transform.childCount); //140
        for(int i = 0; i < transform.childCount; i++)
        {
            zombies[i] = transform.GetChild(i).gameObject;
        }
    }

    private void Start()
    {
        // Set Active 0-10
        for (int i = 0; i < 10; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 10-20
        for (int i = 10; i < 20; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 20-30
        for (int i = 20; i < 30; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 30-40
        for (int i = 30; i < 40; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 40-50
        for (int i = 40; i < 50; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 50-60
        for (int i = 50; i < 60; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 60-70
        for (int i = 60; i < 70; i++)
        {
            zombies[i].SetActive(true);
        }
    }

    private void Update()
    {
        // Set Active 70-80
        for (int i = 70; i < 80; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 80-90
        for (int i = 80; i < 90; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 90-100
        for (int i = 90; i < 100; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 100-110
        for (int i = 100; i < 110; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 110-120
        for (int i = 110; i < 120; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 120-130
        for (int i = 120; i < 130; i++)
        {
            zombies[i].SetActive(true);
        }
        // Set Active 130-140
        for (int i = 130; i < 140; i++)
        {
            zombies[i].SetActive(true);
        }
    }
}
