using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenuManager : MonoBehaviour
{
    // Switch obejct active to on/off
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
