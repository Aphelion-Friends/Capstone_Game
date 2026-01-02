using UnityEngine;
using System.Collections.Generic;
using System;

namespace MapBuilder
{
    [Serializable]
    public class Piece
    {
        [SerializeField] private string _prefabName;
        public string prefabName { get => _prefabName; }

        public Piece(string newPrefabName)
        {
            _prefabName = newPrefabName;
        }

        public Piece()
        {
            _prefabName = "default";
        }
    }
}
