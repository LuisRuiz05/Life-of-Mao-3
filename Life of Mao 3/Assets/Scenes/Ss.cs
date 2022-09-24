using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ss : MonoBehaviour
{
    public InputAction inputAction;

    private void OnEnable()
    {
        inputAction.Enable();
    }

    void Update()
    {
        if (inputAction.ReadValue<float>() > 0.1f)
        {
            Debug.Log(inputAction.ReadValue<float>());
        }
    }
}
