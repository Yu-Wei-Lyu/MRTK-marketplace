using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWindowManager : MonoBehaviour
{
    [Tooltip("The menu plate object")]
    public GameObject primaryPage;
    [Tooltip("The product plate object")]
    public GameObject secondaryPage;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        this.SwitchToPrimaryPage();
    }

    // Switch obejct active to on/off
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // Switch form to menu plate
    private void SwitchToPrimaryPage()
    {
        primaryPage.SetActive(true);
        secondaryPage.SetActive(false);
    }
}
