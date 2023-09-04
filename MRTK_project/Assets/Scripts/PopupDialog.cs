using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class PopupDialog : MonoBehaviour
    {
        public enum Response
        {
            Confirm,
            Cancel,
        }

        private Action<Response, int> _responseCallback;

        [SerializeField]
        private TextMeshProUGUI _titleTextLabel;
        [SerializeField]
        private TextMeshProUGUI _messageTextLabel;
        [SerializeField]
        private RectTransform _rebuilderUtilityParentTarget;
        [SerializeField]
        private GameObject _loadingIcon;
        [SerializeField]
        private GameObject _counterZone;
        [SerializeField]
        private GameObject _confirmButton;
        [SerializeField]
        private GameObject _cancelButton;

        private List<GameObject> _deactivatedList;
        private AudioSource _audioSource;
        private CounterController _counter;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            _deactivatedList = new List<GameObject>();
            _counter = _counterZone.GetComponent<CounterController>();
            SetDisplayMode(DialogDisplayOptions.Default);
            _counter.ResetCounter();
        }

        // Set dialog display mode
        private void SetDisplayMode(DialogDisplayOptions options)
        {
            _loadingIcon.SetActive(options.ShowLoadingIcon);
            _counterZone.SetActive(options.ShowCounterZone);
            _confirmButton.SetActive(options.ShowConfirmButton);
            _cancelButton.SetActive(options.ShowCancelButton);
        }

        // Rebuild layout by parent game object
        public void RebuildLayout()
        {
            LayoutRebuilderUtility.RebuildLayoutsWithContentSizeFitter(_rebuilderUtilityParentTarget);
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

        // Close dialog
        public void CloseDialog()
        {
            if (_audioSource != null)
            {
                StartCoroutine(WaitForSoundPlayed());
            }
            SetActive(false);
        }

        // Set title and message
        public void SetTexts(string titleString, string messageString = null)
        {
            var messageTextObject = _messageTextLabel.gameObject;
            _titleTextLabel.text = titleString;
            if (messageString == null)
            {
                messageTextObject.SetActive(false);
            }
            else
            {
                _messageTextLabel.text = messageString;
                messageTextObject.SetActive(true);
            }
            RebuildLayout();
        }

        // Set activation
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
            if (_deactivatedList.Count != 0)
            {
                if (value)
                {
                    _deactivatedList.ForEach(targetObject => targetObject.SetActive(false));
                    _counter.ResetCounter();
                }
                else
                {
                    _deactivatedList.ForEach(targetObject => targetObject.SetActive(true));
                    _deactivatedList.Clear();
                }
            }
            RebuildLayout();
        }

        // Add associated gameobject to deactivated later
        public void AddToBeDeactived(GameObject gameObject)
        {
            _deactivatedList.Add(gameObject);
        }

        // Display a dialog box with loaded icons and text
        public void LoadingDialog(string title, string message = null)
        {
            SetTexts(title, message);
            SetDisplayMode(DialogDisplayOptions.Loading);
            SetActive(true);
        }

        // Display a dialog box with confirmation options and text
        public void ConfirmDialog(string title, string message = null)
        {
            SetTexts(title, message);
            SetDisplayMode(DialogDisplayOptions.Confirm);
            SetActive(true);
        }

        // Displaying the dialog and waiting for response before calling the callback function
        public void WaitingResponseDialog(Action<Response, int> callback, bool enabledCounter = false)
        {
            if (enabledCounter)
                SetDisplayMode(DialogDisplayOptions.CounterReturnOrCancel);
            else
                SetDisplayMode(DialogDisplayOptions.ConfirmOrCancel);
            SetActive(true);
            _responseCallback = callback;
        }

        // On confirm button click
        public void OnConfirmButtonClicked()
        {
            SetActive(false);
            var quantity = _counter.GetValue();
            _responseCallback?.Invoke(Response.Confirm, quantity);
            _responseCallback = null;
        }

        // On cancel button click
        public void OnCancelButtonClicked()
        {
            SetActive(false);
            _responseCallback?.Invoke(Response.Cancel, 0);
            _responseCallback = null;
        }
    }
}