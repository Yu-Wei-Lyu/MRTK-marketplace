using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;

public class SlateController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro titlebarTextMeshPro;
    [SerializeField]
    private GameObject closeButton;
    [SerializeField]
    private PressableButton followPressableButton;
    [SerializeField]
    private GameObject backButton;
    [SerializeField]
    private List<Plate> plates;

    private Plate currentPlate;
    private Plate primaryPlate;
    private Stack<Plate> previousPlateStack = new Stack<Plate>();
    private const int primaryPlateIndex = 0;
    private const int secondaryPlateBeginIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        this.backButton.SetActive(false);
        this.followPressableButton.ForceSetToggled(true);
        InitializePlate();
    }

    // Initialize plate
    private void InitializePlate()
    {
        this.primaryPlate = this.plates[primaryPlateIndex];
        this.primaryPlate.SetActive(true);
        this.titlebarTextMeshPro.text = this.primaryPlate.Title;
        this.currentPlate = this.primaryPlate;
        for (int index = secondaryPlateBeginIndex; index < this.plates.Count; ++index)
        {
            this.plates[index].SetActive(false);
        }
    }

    // Return the Plate object that contains the target plate, not found will be null.
    private Plate GetPlateIfContains(GameObject targetPlate)
    {
        return this.plates.Find(plate => plate.IsSame(targetPlate));
    }

    // Activate target plate and deactivate other plates
    public void SwitchToPlate(GameObject targetPlate)
    {
        this.currentPlate.SetActive(false);
        this.previousPlateStack.Push(this.currentPlate);
        this.currentPlate = this.GetPlateIfContains(targetPlate);
        this.currentPlate.SetActive(true);
        this.titlebarTextMeshPro.text = this.currentPlate.Title;
        if (currentPlate == this.primaryPlate)
        {
            this.previousPlateStack.Clear();
            this.backButton.SetActive(false);
        }
        else
        {
            this.backButton.SetActive(true);
        }
    }

    // Activate Previous plate and deactivate current plates
    public void SwitchToPreviousPlate()
    {
        this.currentPlate.SetActive(false);
        this.currentPlate = this.previousPlateStack.Pop();
        this.currentPlate.SetActive(true);
        this.titlebarTextMeshPro.text = currentPlate.Title;
        if (this.currentPlate == this.primaryPlate)
        {
            this.backButton.SetActive(false);
        }
        else
        {
            this.backButton.SetActive(true);
        }
    }

    // Do something before setting activated
    public void SetActive(bool value)
    {
        if (value)
        {
            this.followPressableButton.ForceSetToggled(true);
        }
        this.gameObject.SetActive(value);
    }
}
