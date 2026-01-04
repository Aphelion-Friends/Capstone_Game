using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace MapBuilder
{
    // This class is responsible for loading the map into the game based on a Map object
    public class MapInitializer : MonoBehaviour
    {
        private AddressablesLoader addressablesLoader;

        [SerializeField] private string mapName;
        // If you attach this script to a GameObject and you want to load the map when the game starts, set this to true
        [SerializeField] private bool autoInitialize = false;

        private Map map;

        // List of all map piece addressable names
        private List<string> keys;
        // Maps the addressable name of a map piece to its MapPiece object
        private Dictionary<string, MapPiece> mapPieceDictionary;

        // If _editMode is true, a box collider will be created for each map piece
        private bool _editMode = false;

        public void Initialize(Map newMap, bool editMode)
        {
            _editMode = editMode;
            map = newMap;
            keys = GetAllMapPrefabNames();
            Debug.Log($"Num keys: {keys.Count}");
            mapPieceDictionary = GetMapPieceDictionary();
            addressablesLoader.LoadAssets(keys);
        }

        public void Initialize(Map newMap)
        {
            this.Initialize(newMap, false);
        }

        void Awake()
        {
            addressablesLoader = gameObject.AddComponent<AddressablesLoader>();
            addressablesLoader.Ready.AddListener(OnAssetsReady);

            if (autoInitialize)
            {
                MapFileStorage mapFileStorage = new MapFileStorage();
                Map mapFromFile = mapFileStorage.ReadMapFromFile(mapName);
                Initialize(mapFromFile);
            }
        }

        // This function is called after the addressables finish loading
        private void OnAssetsReady()
        {
            foreach (var mapPiece in map.pieces)
            {
                // Create all the map piece GameObjects and set their position and rotation
                Vector3 position = new Vector3(mapPiece.location.x * map.gridUnitSize, mapPiece.location.y * map.gridUnitSize, mapPiece.location.z * map.gridUnitSize);
                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(0, mapPiece.orientation * 90, 0);

                GameObject newPiece = Instantiate(addressablesLoader.operationDictionary[mapPiece.piece.prefabName].Result, position, rotation);

                // Create the box colliders
                if (_editMode)
                {
                    GameObject newCollider = new GameObject("Collider");
                    newCollider.AddComponent<BoxCollider>();
                    newCollider.transform.parent = newPiece.transform;
                    newCollider.transform.position = new Vector3(
                            newPiece.transform.position.x,
                            newPiece.transform.position.y + map.gridUnitSize/2,
                            newPiece.transform.position.z);
                    newCollider.GetComponent<BoxCollider>().size = new Vector3(map.gridUnitSize, map.gridUnitSize, map.gridUnitSize);
                }
            }
        }
    
        private List<string> GetAllMapPrefabNames()
        {
            List<string> listOfNames = new List<string>();

            foreach (MapPiece mapPiece in map.pieces)
            {
                listOfNames.Add(mapPiece.piece.prefabName);
            }

            return listOfNames;
        }

        private Dictionary<string, MapPiece> GetMapPieceDictionary()
        {
            Dictionary<string, MapPiece> mapPieceDictionary = new Dictionary<string, MapPiece>();

            foreach (MapPiece mapPiece in map.pieces)
            {
                mapPieceDictionary[mapPiece.piece.prefabName] = mapPiece;
            }

            return mapPieceDictionary;
        }
    }
}
