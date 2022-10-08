using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHead : MonoBehaviour
{
    public ZombieIA ai;

    void Start()
    {
        ai = gameObject.GetComponentInParent<ZombieIA>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
