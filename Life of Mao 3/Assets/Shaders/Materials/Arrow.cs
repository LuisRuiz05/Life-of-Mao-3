using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform objective;

    void Update()
    {
        Vector3 targetPosition = objective.position;
        targetPosition.y = transform.position.y;

        transform.LookAt(targetPosition);
        transform.Rotate(0f,90f,0f);
    }
}
