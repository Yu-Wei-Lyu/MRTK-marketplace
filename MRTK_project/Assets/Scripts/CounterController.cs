using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class CounterController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _counterTextObject;
        [SerializeField]
        private GameObject _decreaseButton;
        [SerializeField]
        private GameObject _increaseButton;

        private int _counterValue = 1;

        // Get counter value
        public int GetValue()
        {
            return _counterValue;
        }

        // Update the text of the counter 
        private void UpdateCounter()
        {
            _decreaseButton.SetActive(_counterValue > 1);
            _counterTextObject.text = _counterValue.ToString();
        }

        // Reset the text of the counter to zero
        public void ResetCounter()
        {
            _counterValue = 1;
            UpdateCounter();
        }

        // Increase the counter value
        public void IncreaseValue()
        {
            _counterValue += 1;
            UpdateCounter();
        }

        // Decrease the counter value
        public void DecreaseValue()
        {
            if (_counterValue > 1)
                _counterValue -= 1;
            UpdateCounter();
        }
    }
}