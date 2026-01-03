using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MapBuilder
{
    // This class is responsible for storing the map. It is Serializable because it needs to get stored as json.
    [Serializable]
    public class Map
    {
        // _rootWorldPosition isn't being used yet, but it will be handy if we want to load multiple maps in the same scene at some point.
        [SerializeField] private Vector3 _rootWorldPosition;
        // The size of each piece on the 3D grid.
        [SerializeField] private float _gridUnitSize;
        // All of the pieces in the map.
        [SerializeField] private List<MapPiece> _pieces;

        public float gridUnitSize { get => _gridUnitSize; }
        public List<MapPiece> pieces { get => _pieces; }

        // Simply appends a MapPiece to _pieces
        public void AddMapPiece(MapPiece newMapPiece)
        {
            MapPiece mapPieceCopy = new MapPiece(newMapPiece);
            _pieces.Add(mapPieceCopy);
        }

        // Searches through the _pieces list and deletes the first piece with the given coordinates.
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

        // This sorts the pieces according to the Morton encoding of their coordinates.
        // The reason we sort them this way is to preserve the locality of the pieces.
        // So, pieces that are near each other on the map are very likely to be near each other in the one-dimensional list of pieces.
        // This supports collaboration on the map with git because if two people make changes to different parts of the map, their changes will likely merge automatically.
        // However, if two people make changes to the same area of the map, a merge conflict will likely be generated, which requires manual merging.
        public void SortPieces()
        {
            // Use OrderBy instead of Sort because OrderBy uses a stable sorting algorithm.
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
