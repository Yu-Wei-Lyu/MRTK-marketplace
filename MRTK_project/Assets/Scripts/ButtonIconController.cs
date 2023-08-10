using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class ButtonIconController : MonoBehaviour
{
    [SerializeField]
    private bool isToggle;
    [SerializeField]
    private Texture IconActive;
    [SerializeField]
    private Texture IconDefault;

    private ButtonConfigHelper buttonConfigHelper;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        this.buttonConfigHelper = this.GetComponent<ButtonConfigHelper>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.UpdateDisplay();
    }

    // Update and display button icon texture
    public void UpdateDisplay()
    {
        if (this.isToggle)
        {
            this.buttonConfigHelper.SetQuadIcon(this.IconActive);
        }
        else
        {
            this.buttonConfigHelper.SetQuadIcon(this.IconDefault);
        }
    }

    // Toggle button state
    public void ToggleState()
    {
        this.isToggle = !this.isToggle;
        this.UpdateDisplay();
    }

    // Force setting state
    public void ForceSetToggle(bool value)
    {
        this.isToggle = value;
        this.UpdateDisplay();
    }
}
