using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class DialogBase : MonoBehaviour
    {
        public List<GameObject> DeactivatedList 
        { 
            get;
            set; 
        }
        public bool KeepDeactiveList
        {
            get;
            set;
        } = false;

        // Awake is called when the script instance is being loaded.
        public virtual void Awake()
        {
            DeactivatedList = new List<GameObject>();
        }

        // Set the activation state of this gameObject
        public virtual void SetActive(bool value)
        {
            if (KeepDeactiveList)
            {
                return;
            }
            if (DeactivatedList.Count != 0)
            {
                if (value)
                {
                    DeactivatedList.ForEach(targetObject => targetObject.SetActive(false));
                }
                else
                {
                    DeactivatedList.ForEach(targetObject => targetObject.SetActive(true));
                    DeactivatedList.Clear();
                }
            }
        }

        // Add associated gameobject to deactivated later
        public virtual void AddToBeDeactived(GameObject gameObject)
        {
            DeactivatedList.Add(gameObject);
        }
    }
}