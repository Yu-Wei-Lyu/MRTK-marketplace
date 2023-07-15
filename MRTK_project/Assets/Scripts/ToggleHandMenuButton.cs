using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class ToggleHandMenuButton : MonoBehaviour
{
    private bool isMenuActive;
    // Start is called before the first frame update
    void Start()
    {
        isMenuActive = !(GameObject.Find("MenuContent") == null);
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Display button appearance
    private void UpdateDisplay()
    {
        ButtonConfigHelper buttonConfigHelper = gameObject.GetComponent<ButtonConfigHelper>();
        if (isMenuActive)
        {
            buttonConfigHelper.SetQuadIconByName(IconNameSet.LightbulbOn);
        }
        else
        {
            buttonConfigHelper.SetQuadIconByName(IconNameSet.LightbulbOff);
        }
    }

    // Toggle the state of the button
    public void ToggleDisplay()
    {
        isMenuActive = !isMenuActive;
        UpdateDisplay();
    }
}
