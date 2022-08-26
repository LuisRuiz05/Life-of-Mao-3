using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
///     This class acts as the tooltip controller.
/// </summary>
public class Tooltip : MonoBehaviour
{
    public Text tooltipText;
    private Image myImage;
    private bool isHovering;
    
    private void Start()
    {
        myImage = GetComponent<Image>();
        myImage.enabled = false;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="itemName"> Displayed name in the tooltip. </param>
    public void SetToolTip(string itemName)
    {
        if(itemName.Length > 0)
        {
            isHovering = true;
            tooltipText.text = itemName;
            myImage.enabled = true;
        }
        else
        {
            tooltipText.text = string.Empty;
            myImage.enabled = false;
            isHovering = false;
        }
    }

    private void Update()
    {
        if (isHovering)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }
}
