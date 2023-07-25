using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseFormBehavior : MonoBehaviour
{
    [Tooltip("Parent of the object that presents the entired page")]
    public GameObject ParentWindow;
    [Tooltip("A button that triggers the window")]
    public GameObject WindowTriggerButton;

    private AudioSource audioSource;
    private ButtonIconController triggerButtonController;

    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        triggerButtonController = WindowTriggerButton.GetComponent<ButtonIconController>();
    }

    // The event when button released
    public void OnRelease()
    {
        StartCoroutine(WaitForSoundPlayed());
        ParentWindow.SetActive(false);
        triggerButtonController.UpdateDisplay();
    }

    // Wait for sound played
    private IEnumerator WaitForSoundPlayed()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
    }
}
