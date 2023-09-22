using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class CounterController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _counterTextObject;
        [SerializeField]
        private GameObject _numberInputBox;

        private int _counterValue = 0;

        public int Value
        {
            get => _counterValue;
            set
            {
                _counterValue = value;
                _counterTextObject.text = _counterValue.ToString();
            }
        }

        // Initialize the state of counter, and reset the value to 0
        public void Initialize()
        {
            Value = 0;
        }

        // Enable the number box area
        public void ActivatedNumberInputBox(bool state)
        {
            _numberInputBox.SetActive(state);
        }

        // Number button OnPressed event
        public void NumberButtonClicked(int number)
        {
            Value = Value * 10 + number;
        }

        // Back button OnPressed event
        public void DeleteButtonClicked()
        {
            Value /= 10;
        }

        // Reset button OnPress event
        public void ClearButtonClicked()
        {
            Value = 0;
        }
    }
}