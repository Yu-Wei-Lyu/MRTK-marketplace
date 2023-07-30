using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plate
{
    [SerializeField]
    private string title;
    [SerializeField]
    private GameObject plate;

    // Get plate title
    public string Title
    {
        get { return this.title; }
    }

    // Set the plate activation state
    public void SetActive(bool value)
    {
        this.plate.SetActive(value);
    }

    // Set the plate position
    public void SetPosition(Vector3 newPosition)
    {
        this.plate.transform.position = newPosition;
    }

    // Compare plates
    public bool IsSame(GameObject targetPlate)
    {
        return this.plate == targetPlate;
    }
}
