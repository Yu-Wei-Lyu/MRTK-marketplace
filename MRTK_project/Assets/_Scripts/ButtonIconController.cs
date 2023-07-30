using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class ButtonIconController : MonoBehaviour
{
    [Tooltip("The object associated with the button")]
    public GameObject AssociatedObject;
    [Tooltip("The icon to display when the associated object is activate")]
    public Texture IconActive;
    [Tooltip("The icon to display when the associated object is deactivated")]
    public Texture IconDefault;
    private ButtonConfigHelper buttonConfigHelper;
    // Start is called before the first frame update
    void Start()
    {
        buttonConfigHelper = GetComponent<ButtonConfigHelper>();
        UpdateDisplay();
    }

    // Update and display button icon texture
    public void UpdateDisplay()
    {
        if (AssociatedObject.activeSelf)
        {
            buttonConfigHelper.SetQuadIcon(IconActive);
        }
        else
        {
            buttonConfigHelper.SetQuadIcon(IconDefault);
        }
    }
}
