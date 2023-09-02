using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class ButtonIconController : MonoBehaviour
    {
        [SerializeField]
        private bool _isToggle;
        [SerializeField]
        private Texture _iconActive;
        [SerializeField]
        private Texture _iconDefault;

        private ButtonConfigHelper _buttonConfigHelper;

        // Awake is called when the script instance is being loaded.
        public void Awake()
        {
            _buttonConfigHelper = GetComponent<ButtonConfigHelper>();
        }

        // Start is called before the first frame update
        public void Start()
        {
            UpdateDisplay();
        }

        // Update and display button icon texture
        public void UpdateDisplay()
        {
            _buttonConfigHelper.SetQuadIcon((_isToggle) ? _iconActive : _iconDefault);
        }

        // Toggle button state
        public void ToggleState()
        {
            _isToggle = !_isToggle;
            UpdateDisplay();
        }

        // Force setting state
        public void ForceToggle(bool value)
        {
            _isToggle = value;
            UpdateDisplay();
        }
    }
}