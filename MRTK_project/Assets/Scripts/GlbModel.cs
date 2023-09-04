using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GlbModel
    {
        private readonly int _id;
        private readonly GameObject _model;

        public GlbModel(int id, GameObject model)
        {
            _id = id;
            _model = model;
        }

        public int FurnitureID => _id;

        public GameObject ModelObject => _model;
    }
}