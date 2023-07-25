using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPageBehavior : MonoBehaviour
{
    [Tooltip("Current page object")]
    public GameObject CurrentPage;
    [Tooltip("Switch to target page object")]
    public GameObject TargetPage;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // The event when button released
    public void OnRelease()
    {
        StartCoroutine(WaitForSoundPlayed());
        SwitchToPreviousPage();
    }

    // Wait for sound played
    private IEnumerator WaitForSoundPlayed()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
    }

    // Switch to previous page
    private void SwitchToPreviousPage()
    {
        CurrentPage.SetActive(false);
        TargetPage.SetActive(true);
    }
}
