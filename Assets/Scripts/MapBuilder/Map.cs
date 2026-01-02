using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MapBuilder
{
    [Serializable]
    public class Map
    {
        [SerializeField] private Vector3 _rootWorldPosition;
        [SerializeField] private float _gridUnitSize;
        [SerializeField] private List<MapPiece> _pieces;

        public float gridUnitSize { get => _gridUnitSize; }
        public List<MapPiece> pieces { get => _pieces; }

        public void AddMapPiece(MapPiece newMapPiece)
        {
            MapPiece mapPieceCopy = new MapPiece(newMapPiece);
            _pieces.Add(mapPieceCopy);
        }

        public void DeleteMapPieceByLocation(Vector3Int location)
        {
            foreach(MapPiece mapPiece in _pieces)
            {
                if (mapPiece.location == location)
                {
                    _pieces.Remove(mapPiece);
                    return;
                }
            }
        }

        public void SortPieces()
        {
             IOrderedEnumerable<MapPiece> sortedPieces = _pieces.OrderBy(piece => piece.GetMortonCode());
             List<MapPiece> sortedPiecesList = new List<MapPiece>();
             foreach (MapPiece piece in sortedPieces)
             {
                 sortedPiecesList.Add(piece);
             }
             _pieces = sortedPiecesList;
        }

        public Map(float newGridUnitSize)
        {
            _gridUnitSize = newGridUnitSize;
        }

        public Map()
        {
            _gridUnitSize = 0f;
            _rootWorldPosition = new Vector3();
            _pieces = new List<MapPiece>();
        }

        public Map(float newGridUnitSize, List<MapPiece> newMapPieces)
        {
            _gridUnitSize = newGridUnitSize;
            _pieces = new List<MapPiece>();
            _pieces = newMapPieces;
        }
    }
}
