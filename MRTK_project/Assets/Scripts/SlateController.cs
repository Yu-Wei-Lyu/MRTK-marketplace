using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using TMPro;
using UnityEngine;

public class SlateController : MonoBehaviour
{
    [SerializeField]
    private DataManager _dataManager;
    [SerializeField]
    private TextMeshPro _titlebarText;
    [SerializeField]
    private ButtonIconController _followButton;
    [SerializeField]
    private GameObject _backButton;
    [SerializeField]
    private List<Plate> _plates;

    private Plate _currentPlate;
    private Plate _primaryPlate;
    private Stack<Plate> _previousPlateStack = new Stack<Plate>();
    private RadialView _radialView;
    private AudioSource _audioSource = null;
    private const int PRIMARY_PLATE_INDEX = 0;
    private const int SECONDARY_PLATE_BEGIN_INDEX = 1;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        _radialView = this.GetComponentInChildren<RadialView>();
    }

    // Start is called before the first frame updates
    void Start()
    {
        _backButton.SetActive(false);
        InitializePlate();
    }

    // Initialize plate
    private void InitializePlate()
    {
        _primaryPlate = _plates[PRIMARY_PLATE_INDEX];
        _primaryPlate.SetActive(true);
        _titlebarText.text = _primaryPlate.Title;
        _currentPlate = _primaryPlate;
        _followButton.ForceToggle(true);
        for (int index = SECONDARY_PLATE_BEGIN_INDEX; index < _plates.Count; ++index)
        {
            _plates[index].SetActive(false);
        }
    }

    // Return the Plate object that contains the target plate, not found will be null.
    private Plate GetPlateIfContains(GameObject targetPlate)
    {
        return _plates.Find(plate => plate.IsSameReference(targetPlate));
    }

    // Wait for sound played
    private IEnumerator WaitForSoundPlayed()
    {
        if (_audioSource.isPlaying)
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
            _audioSource = null;
        }
    }

    // SetAudioSourcePlaying
    public void SetAudioSourcePlaying(AudioSource source)
    {
        _audioSource = source;
    }

    // Activate target plate and deactivate other plates
    public void SwitchToPlate(GameObject targetPlate)
    {
        if (_audioSource != null)
        {
            StartCoroutine(WaitForSoundPlayed());
        }
        if (_currentPlate.IsSameReference(targetPlate))
        {
            return;
        }
        _currentPlate.SetActive(false);
        _previousPlateStack.Push(_currentPlate);
        _currentPlate = this.GetPlateIfContains(targetPlate);
        _currentPlate.SetActive(true);
        _titlebarText.text = _currentPlate.Title;
        if (_currentPlate == _primaryPlate)
        {
            _previousPlateStack.Clear();
            _backButton.SetActive(false);
        }
        else
        {
            _backButton.SetActive(true);
        }
    }

    // Activate Previous plate and deactivate current plates
    public void SwitchToPreviousPlate()
    {
        if (_audioSource != null)
        {
            StartCoroutine(WaitForSoundPlayed());
        }
        _currentPlate.SetActive(false);
        _currentPlate = _previousPlateStack.Pop();
        _currentPlate.SetActive(true);
        _titlebarText.text = _currentPlate.Title;
        if (_currentPlate == _primaryPlate)
        {
            _backButton.SetActive(false);
            _dataManager.ResetRecentlyQueriedIndex();
        }
        else
        {
            _backButton.SetActive(true);
        }
    }

    // Do something before setting activated
    public void SetActive(bool value)
    {
        if (_audioSource != null && this.isActiveAndEnabled)
        {
            StartCoroutine(WaitForSoundPlayed());
        }
        this.gameObject.SetActive(value);
        if (value)
        {
            _radialView.enabled = true;
            _followButton.ForceToggle(true);
        }
    }

    // Toggle the radial view state
    public void ToggleRadialViewState()
    {
        _radialView.enabled = !_radialView.enabled;
    }
}
