using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class ToggleGrayCubeButton : MonoBehaviour
{
    private bool isCubeActive;
    // Start is called before the first frame update
    void Start()
    {
        isCubeActive = !(GameObject.Find("ItemMenu") == null);
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
        if (isCubeActive)
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
        isCubeActive = !isCubeActive;
        UpdateDisplay();
    }
}
