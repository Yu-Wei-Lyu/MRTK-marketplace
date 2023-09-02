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

        // Awake is called when the script instance is being loaded.
        public virtual void Awake()
        {
        }

        // Start is called before the first frame update
        public virtual void Start()
        {
        }

        // Update is called once per frame
        public virtual void Update()
        {
        }

        // This function is called when the object becomes enabled and active.
        public virtual void OnEnable()
        {

        }

        // This function is called when the object becomes disabled or inactive.
        public virtual void OnDisable()
        {
        }

        // This function is called when the MonoBehaviour will be destroyed.
        public virtual void OnDestroy()
        {
        }

        // Initialize (virtual function)
        public virtual void Initialize()
        {
        }

        // Set the plate activation state
        public virtual void SetActive(bool value)
        {
            if (value)
                Initialize();
            gameObject.SetActive(value);
        }

        // Compare plates
        public bool IsSameReference(GameObject targetPlate)
        {
            return ReferenceEquals(gameObject, targetPlate);
        }
    }
}