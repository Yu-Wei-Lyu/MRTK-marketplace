using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;

public class SlateController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro titlebarTextMeshPro;
    [SerializeField]
    private GameObject closeButton;
    [SerializeField]
    private ButtonIconController followButton;
    [SerializeField]
    private GameObject backButton;
    [SerializeField]
    private List<Plate> plates;

    private Plate currentPlate;
    private Plate primaryPlate;
    private Stack<Plate> previousPlateStack = new Stack<Plate>();
    private RadialView radialView;
    private AudioSource audioSource;
    private const int primaryPlateIndex = 0;
    private const int secondaryPlateBeginIndex = 1;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        this.radialView = this.GetComponentInChildren<RadialView>();
    }

    // Start is called before the first frame updates
    void Start()
    {
        this.backButton.SetActive(false);
        InitializePlate();
    }

    // Initialize plate
    private void InitializePlate()
    {
        this.primaryPlate = this.plates[primaryPlateIndex];
        this.primaryPlate.SetActive(true);
        this.titlebarTextMeshPro.text = this.primaryPlate.Title;
        this.currentPlate = this.primaryPlate;
        this.followButton.ForceSetToggle(true);
        for (int index = secondaryPlateBeginIndex; index < this.plates.Count; ++index)
        {
            this.plates[index].SetActive(false);
        }
    }

    // Return the Plate object that contains the target plate, not found will be null.
    private Plate GetPlateIfContains(GameObject targetPlate)
    {
        return this.plates.Find(plate => plate.IsSameReference(targetPlate));
    }

    // Wait for sound played
    private IEnumerator WaitForSoundPlayed()
    {
        if (this.audioSource.isPlaying)
        {
            yield return new WaitWhile(() => this.audioSource.isPlaying);
            this.audioSource = null;
        }
    }

    // SetAudioSourcePlaying
    public void SetAudioSourcePlaying(AudioSource source)
    {
        this.audioSource = source;
    }

    // Activate target plate and deactivate other plates
    public void SwitchToPlate(GameObject targetPlate)
    {
        if (this.audioSource != null)
        {
            StartCoroutine(WaitForSoundPlayed());
        }
        if (this.currentPlate.IsSameReference(targetPlate))
        {
            return;
        }
        this.currentPlate.SetActive(false);
        this.previousPlateStack.Push(this.currentPlate);
        this.currentPlate = this.GetPlateIfContains(targetPlate);
        this.currentPlate.SetActive(true);
        this.titlebarTextMeshPro.text = this.currentPlate.Title;
        if (this.currentPlate == this.primaryPlate)
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
        if (this.audioSource != null)
        {
            StartCoroutine(WaitForSoundPlayed());
        }
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
        if (this.audioSource != null && this.isActiveAndEnabled)
        {
            StartCoroutine(WaitForSoundPlayed());
        }
        this.gameObject.SetActive(value);
        if (value)
        {
            this.radialView.enabled = true;
            this.followButton.ForceSetToggle(true);
        }
    }

    // Toggle the radial view state
    public void ToggleRadialViewState()
    {
        this.radialView.enabled = !this.radialView.enabled;
    }
}
