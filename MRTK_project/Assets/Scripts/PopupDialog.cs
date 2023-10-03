using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class PopupDialog : DialogBase
    {
        private const string AUTO_CLOSE_MESSAGE = "對話將自動關閉";
        private const double CLOSE_DELAY_SECOND = 1.5;
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
        private GameObject _responseButtonsParent;
        [SerializeField]
        private GameObject _confirmButton;
        [SerializeField]
        private GameObject _cancelButton;

        private CounterController _counter;

        // Awake is called when the script instance is being loaded.
        public override void Awake()
        {
            base.Awake();
            _counter = _counterZone.GetComponent<CounterController>();
            SetDisplayMode(DialogDisplayOptions.Default);
            _counter.Initialize();
            gameObject.SetActive(false);
        }

        // Set dialog display mode
        private void SetDisplayMode(DialogDisplayOptions options)
        {
            _loadingIcon.SetActive(options.ShowLoadingIcon);
            _counterZone.SetActive(options.ShowCounterZone);
            _counter.ActivatedNumberInputBox(options.ShowCounterZone);
            _confirmButton.SetActive(options.ShowConfirmButton);
            _cancelButton.SetActive(options.ShowCancelButton);
            _responseButtonsParent.SetActive(options.ShowConfirmButton || options.ShowCancelButton);
        }

        // Rebuild layout by parent game object
        public void RebuildLayout()
        {
            LayoutRebuilderUtility.RebuildLayoutsWithContentSizeFitter(_rebuilderUtilityParentTarget);
        }

        // Set title and message
        public void SetTexts(string titleString, string messageString = null)
        {
            GameObject messageTextObject = _messageTextLabel.gameObject;
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

        // Set the activation state of this gameObject
        public override void SetActive(bool value)
        {
            base.SetActive(value);
            if (value)
            {
                _counter.Initialize();
            }
            gameObject.SetActive(value);
            RebuildLayout();
        }

        // Display a dialog box with texts
        public void TextDialog(string title, string message = null)
        {
            SetTexts(title, message);
            SetDisplayMode(DialogDisplayOptions.Default);
            SetActive(true);
        }

        // Display a dialog box with loaded icons and texts
        public void LoadingDialog(string title, string message = null)
        {
            SetTexts(title, message);
            SetDisplayMode(DialogDisplayOptions.Loading);
            SetActive(true);
        }

        // Display a dialog box with confirmation options and texts
        public void ConfirmDialog(string title, string message = null)
        {
            SetTexts(title, message);
            SetDisplayMode(DialogDisplayOptions.Confirm);
            SetActive(true);
        }

        // Invoking the callback function when the dialog is confirmed or canceled
        public void WaitingResponseDialog(Action<Response, int> callback, bool enabledCounter = false)
        {
            if (enabledCounter)
            {
                SetDisplayMode(DialogDisplayOptions.CounterReturnOrCancel);
            }
            else
            {
                SetDisplayMode(DialogDisplayOptions.ConfirmOrCancel);
            }
            SetActive(true);
            _responseCallback = callback;
        }

        // On confirm button click
        public void OnConfirmButtonClicked()
        {
            SetActive(false);
            int quantity = _counter.Value;
            _responseCallback?.Invoke(Response.Confirm, quantity);
            _responseCallback = null;
        }

        // On cancel button click
        public void OnCancelButtonClicked()
        {
            KeepDeactiveList = false;
            SetActive(false);
            _responseCallback?.Invoke(Response.Cancel, 0);
            _responseCallback = null;
        }

        // Close dialog
        public async Task DelayCloseDialog(string title)
        {
            TextDialog(title, AUTO_CLOSE_MESSAGE);
            await Task.Delay(TimeSpan.FromSeconds(CLOSE_DELAY_SECOND));
            KeepDeactiveList = false;
            SetActive(false);
        }
    }
}