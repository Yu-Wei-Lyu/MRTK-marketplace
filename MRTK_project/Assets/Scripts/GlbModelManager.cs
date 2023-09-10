using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GlbModelManager
    {
        private readonly List<GlbModel> _models;
        public int Count => _models.Count;

        public int CacheIndex
        {
            get;
            set;
        }

        public GlbModelManager()
        {
            _models = new List<GlbModel>();
        }

        // Is index in list of model
        private bool IsInList(int index)
        {
            return index >= 0 && index < _models.Count;
        }

        // Get model by index
        public GlbModel GetModelAt(int index)
        {
            GlbModel model = null;
            if (IsInList(index))
            {
                model = _models[index];
            }
            else
            {
                Debug.LogError($"[GlbModelManager] index {index} out of the range");
            }
            return model;
        }

        // Reset cache index
        public void ResetCacheIndex()
        {
            CacheIndex = -1;
        }

        // Get cache model by index
        public GlbModel GetCacheModel()
        {
            return GetModelAt(CacheIndex);
        }

        // Add furnitures
        public void Add(int furnitureID, GameObject modelObject)
        {
            _models.Add(new GlbModel(furnitureID, modelObject));
        }

        // Decrease or delete furniture
        public void Remove(int index)
        {
            if (IsInList(index))
            {
                _models.RemoveAt(index);
            }
            else
            {
                Debug.LogError($"[GlbModelManager] index {index} out of the range");
            }
        }
    }
}