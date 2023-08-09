using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupDialogController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleTextObject;
    [SerializeField]
    private TextMeshProUGUI messageTextObject;
    [SerializeField]
    private RectTransform rebuilderUtilityParentTarget;

    private AudioSource audioSource;
    private string title = null;
    private string message = null;

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

    // 
    public void CloseDialog()
    {
        if (this.audioSource != null)
        {
            StartCoroutine(WaitForSoundPlayed());
        }
        this.SetActive(false);
    }

    // Set title and message
    public void SetTitleMessage(string titleString, string messageString)
    {
        this.title = titleString;
        this.titleTextObject.text = this.title;
        this.message = messageString;
        this.messageTextObject.text = this.message;
    }

    // Set activation
    public void SetActive(bool value)
    {
        if (value)
        {
            if (this.title != null && this.message != null)
            {
                LayoutRebuilderUtility.RebuildLayoutsWithContentSizeFitter(rebuilderUtilityParentTarget);
                this.gameObject.SetActive(value);
            }
            else
            {
                Debug.LogError($"PopupDialog title or main message cannot be null");
            }
        }
        else
        {
            this.title = null;
            this.message = null;
            this.gameObject.SetActive(value);
        }
    }
}
