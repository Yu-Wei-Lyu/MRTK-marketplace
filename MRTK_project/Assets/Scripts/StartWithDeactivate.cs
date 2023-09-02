using UnityEngine;

namespace Assets.Scripts
{
    public class StartWithDeactivate : MonoBehaviour
    {
        // Awake is called when the script instance is being loaded.
        public void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}