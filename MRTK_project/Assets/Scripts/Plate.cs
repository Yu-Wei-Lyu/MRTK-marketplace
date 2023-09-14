using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class Plate : MonoBehaviour
    {
        [SerializeField]
        private string _plateTitle;

        // Get plate title
        public string Title => _plateTitle;

        // Initialize (virtual function)
        public virtual void Initialize() { }

        // Set the plate activation state
        public virtual void SetActive(bool value)
        {
            if (value)
            {
                Initialize();
            }
            gameObject.SetActive(value);
        }

        // Compare plates
        public bool IsSameReference(GameObject targetPlate)
        {
            return ReferenceEquals(gameObject, targetPlate);
        }
    }
}