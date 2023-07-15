using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject itemMenu = GameObject.Find("ItemMenuSlate");
        itemMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
