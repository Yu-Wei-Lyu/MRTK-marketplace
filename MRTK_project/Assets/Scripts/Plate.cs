using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plate : MonoBehaviour
{
    [SerializeField]
    private string plateTitle;

    private GameObject plate;

    // Awake is called when the script instance is being loaded.
    public virtual void Awake()
    {
        this.plate = this.gameObject;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    // This function is called when the object becomes enabled and active.
    public virtual void OnEnable()
    {
        this.Initialize();
    }

    // This function is called when the object becomes disabled or inactive.
    public virtual void OnDisable()
    {

    }

    // This function is called when the MonoBehaviour will be destroyed.
    public virtual void OnDestroy()
    {

    }

    // Get plate title
    public string Title
    {
        get { return this.plateTitle; }
    }

    // Initialize (virtual function)
    public virtual void Initialize() { }

    // Set the plate activation state
    public virtual void SetActive(bool value)
    {
        this.plate.SetActive(value);
    }

    // Set the plate position
    public void SetPosition(Vector3 newPosition)
    {
        this.plate.transform.position = newPosition;
    }

    // Compare plates
    public bool IsSameReference(GameObject targetPlate)
    {
        return ReferenceEquals(this.plate, targetPlate);
    }
}
