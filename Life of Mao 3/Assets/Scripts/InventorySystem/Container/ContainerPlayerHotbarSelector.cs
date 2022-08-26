using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class is in charge of drawing the selector sprite for the hotbar in the UI.
/// </summary>
public class ContainerPlayerHotbarSelector : MonoBehaviour
{
    private PlayerController player;
    private RectTransform myTransform;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        myTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (player != null)
        {
            Vector2 pos = new Vector2((player.GetSelectedHotbarIndex() * 90) + -220, 5); // Initial position 0.
            myTransform.anchoredPosition = pos;
        }
    }
}
