using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Custom class to represent the data structure
[System.Serializable]
public class DataObject
{
    [SerializeField]
    private string name;
    [SerializeField]
    private string price;
    [SerializeField]
    private string size;
    [SerializeField]
    private string material;
    [SerializeField]
    private string manufacturer;
    [SerializeField]
    private string description;
    [SerializeField]
    private Sprite image;

    public string Name
    {
        get { return this.name; }
    }
    public string Price
    {
        get { return this.price; }
    }
    public string Size
    {
        get { return this.size; }
    }
    public string Material
    {
        get { return this.material; }
    }
    public string Manufacturer
    {
        get { return this.manufacturer; }
    }
    public string Description
    {
        get { return this.description; }
    }
    public Sprite Image
    {
        get { return this.image; }
    }
}
