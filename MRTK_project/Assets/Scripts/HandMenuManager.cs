using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Switch obejct active to on/off
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
