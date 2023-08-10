using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWithDeactivate : MonoBehaviour
{
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        gameObject.SetActive(false);
    }
}
