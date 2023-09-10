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

        public int Value { get; set; } = 1;

        // Update the text of the counter 
        private void UpdateCounter()
        {
            _decreaseButton.SetActive(Value > 1);
            _counterTextObject.text = Value.ToString();
        }

        // Reset the text of the counter to zero
        public void ResetCounter()
        {
            Value = 1;
            UpdateCounter();
        }

        // Increase the counter value
        public void IncreaseValue()
        {
            Value += 1;
            UpdateCounter();
        }

        // Decrease the counter value
        public void DecreaseValue()
        {
            if (Value > 1)
                Value -= 1;
            UpdateCounter();
        }
    }
}